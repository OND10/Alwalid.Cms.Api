using Alwalid.Cms.Api.Features.Message.Dtos;

namespace Alwalid.Cms.Api.Features.Conversation.Dtos
{
    public class ChatResponseDto
    {
        public Guid ConversationId { get; set; }
        public MessageDto Message { get; set; } = null!;
    }
}
