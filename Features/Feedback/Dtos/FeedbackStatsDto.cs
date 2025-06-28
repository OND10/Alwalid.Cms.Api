namespace Alwalid.Cms.Api.Features.Feedback.Dtos
{
    public class FeedbackStatsDto
    {
        public int TotalFeedbacks { get; set; }
        public int ActiveFeedbacks { get; set; }
        public double AverageRating { get; set; }
        public int FiveStarCount { get; set; }
        public int FourStarCount { get; set; }
        public int ThreeStarCount { get; set; }
        public int TwoStarCount { get; set; }
        public int OneStarCount { get; set; }
    }
} 