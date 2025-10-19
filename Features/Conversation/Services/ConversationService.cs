
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Features.Gemini.Commands.GenerateContent;
using Alwalid.Cms.Api.Features.Message.Dtos;
using Alwalid.Cms.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Alwalid.Cms.Api.Features.Conversation.Services
{
    public class ConversationService : IConversationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommandHandler<GenerateContentCommand, string> _geminiService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ConversationService(ApplicationDbContext context, ICommandHandler<GenerateContentCommand, string> geminiService, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _geminiService = geminiService;
            _hubContext = hubContext;
        }

        public async Task HandleUserMessageAsync(Guid? conversationId, string userId, string userMessage, CancellationToken cancellationToken)
        {
            Entities.Conversation? conversation;
            if (conversationId.HasValue)
            {
                conversation = await _context.Conversations
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.Id == conversationId.Value && c.ApplicationUserId == userId);

                if (conversation == null) throw new UnauthorizedAccessException("Access to this conversation is denied.");
            }
            else
            {
                conversation = new Entities.Conversation { Topic = "New Conversation", ApplicationUserId = userId };
                _context.Conversations.Add(conversation);
                // We need to save here to get the new conversation ID
                await _context.SaveChangesAsync();
            }

            var newUserMessage = new Entities.Message { Role = "user", Content = userMessage, ConversationId = conversation.Id };
            _context.Messages.Add(newUserMessage);
            await _context.SaveChangesAsync();

            // --- PUSH THE USER'S OWN MESSAGE BACK TO THEM FOR UI SYNC ---
            // This confirms to the client that the message was received and saved.
            var userMessageDto = new MessageDto { Role = newUserMessage.Role, Content = newUserMessage.Content };
            await _hubContext.Clients.Group(conversation.Id.ToString())
                .SendAsync("ReceiveMessage", conversation.Id, userMessageDto);

            // --- GET AI RESPONSE ---
            var history = await _context.Messages
                .Where(m => m.ConversationId == conversation.Id)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            var command = new GenerateContentCommand
            {
                history = history,
            };

            var aiResponseContent = await _geminiService.Handle(command, cancellationToken);

            var aiResponseMessage = new Entities.Message { Role = "model", Content = aiResponseContent.Data, ConversationId = conversation.Id };
            _context.Messages.Add(aiResponseMessage);
            await _context.SaveChangesAsync();

            // --- PUSH THE AI'S RESPONSE VIA SIGNALR ---
            var aiMessageDto = new MessageDto { Role = aiResponseMessage.Role, Content = aiResponseMessage.Content };

            // Send the message to all clients in the conversation's group.
            await _hubContext.Clients.Group(conversation.Id.ToString())
                .SendAsync("ReceiveMessage", conversation.Id, aiMessageDto);
        }

        public async Task<Entities.Conversation?> GetConversationByIdAsync(Guid conversationId, string userId, CancellationToken cancellationToken)
        {
            return await _context.Conversations
                .Include(c => c.Messages.OrderBy(m => m.Timestamp))
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == conversationId && c.ApplicationUserId == userId, cancellationToken);
        }
    }
}
