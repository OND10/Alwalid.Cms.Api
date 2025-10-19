namespace Alwalid.Cms.Api.Features.Conversation.Dtos
{
    public class ChatRequestDto
    {
        public Guid? ConversationId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
