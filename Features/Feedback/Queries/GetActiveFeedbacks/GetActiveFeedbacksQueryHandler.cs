using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Feedback;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Queries.GetActiveFeedbacks
{
    public class GetActiveFeedbacksQueryHandler : IQueryHandler<GetActiveFeedbacksQuery, IEnumerable<FeedbackResponseDto>>
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public GetActiveFeedbacksQueryHandler(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Result<IEnumerable<FeedbackResponseDto>>> Handle(GetActiveFeedbacksQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _feedbackRepository.GetActiveFeedbacksAsync();

                var responseDtos = result.Select(feedback => new FeedbackResponseDto
                {
                    Id = feedback.Id,
                    ArabicName = feedback.ArabicName,
                    EnglishName = feedback.EnglishName,
                    PhoneNumber = feedback.PhoneNumber,
                    ImageUrl = feedback.ImageUrl,
                    Comment = feedback.Comment,
                    Position = feedback.Position,
                    IsActive = feedback.IsActive,
                    Rating = feedback.Rating,
                    CreatedAt = feedback.CreatedAt,
                    LastModifiedAt = feedback.LastModifiedAt
                });

                return await Result<IEnumerable<FeedbackResponseDto>>.SuccessAsync(responseDtos, "Active feedbacks retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<FeedbackResponseDto>>.FaildAsync(false, $"Error retrieving active feedbacks: {ex.Message}");
            }
        }
    }
} 