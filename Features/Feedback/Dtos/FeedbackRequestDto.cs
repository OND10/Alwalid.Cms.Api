namespace Alwalid.Cms.Api.Features.Feedback.Dtos
{
    public class FeedbackRequestDto
    {
        public string ArabicName { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int Rating { get; set; } = 3;

        public IFormFile Image {  get; set; }
    }
} 