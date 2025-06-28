using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Feedback;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Queries.GetFeedbackStats
{
    public class GetFeedbackStatsQueryHandler : IQueryHandler<GetFeedbackStatsQuery, FeedbackStatsDto>
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public GetFeedbackStatsQueryHandler(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Result<FeedbackStatsDto>> Handle(GetFeedbackStatsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var total = await _feedbackRepository.GetTotalCountAsync();
                var active = await _feedbackRepository.GetActiveCountAsync();
                var avg = await _feedbackRepository.GetAverageRatingAsync();
                var five = (await _feedbackRepository.GetByRatingAsync(5)).Count();
                var four = (await _feedbackRepository.GetByRatingAsync(4)).Count();
                var three = (await _feedbackRepository.GetByRatingAsync(3)).Count();
                var two = (await _feedbackRepository.GetByRatingAsync(2)).Count();
                var one = (await _feedbackRepository.GetByRatingAsync(1)).Count();

                var stats = new FeedbackStatsDto
                {
                    TotalFeedbacks = total,
                    ActiveFeedbacks = active,
                    AverageRating = avg,
                    FiveStarCount = five,
                    FourStarCount = four,
                    ThreeStarCount = three,
                    TwoStarCount = two,
                    OneStarCount = one
                };

                return await Result<FeedbackStatsDto>.SuccessAsync(stats, "Feedback statistics retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<FeedbackStatsDto>.FaildAsync(false, $"Error retrieving feedback statistics: {ex.Message}");
            }
        }
    }
} 