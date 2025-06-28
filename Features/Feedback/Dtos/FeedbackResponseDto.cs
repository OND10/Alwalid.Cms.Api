namespace Alwalid.Cms.Api.Features.Feedback.Dtos
{
    public class FeedbackResponseDto
    {
        public int Id { get; set; }
        public string ArabicName { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
} 