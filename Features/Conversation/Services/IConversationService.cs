namespace Alwalid.Cms.Api.Features.Conversation.Services
{
    public interface IConversationService
    {
        Task HandleUserMessageAsync(Guid? conversationId, string userId, string userMessage, CancellationToken cancellationToken = default);
        Task<Entities.Conversation?> GetConversationByIdAsync(Guid conversationId, string userId, CancellationToken cancellationToken=default);
    }
}
