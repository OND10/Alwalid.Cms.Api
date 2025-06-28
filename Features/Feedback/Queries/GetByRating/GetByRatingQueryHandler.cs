using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Feedback;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Queries.GetByRating
{
    public class GetByRatingQueryHandler : IQueryHandler<GetByRatingQuery, IEnumerable<FeedbackResponseDto>>
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public GetByRatingQueryHandler(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Result<IEnumerable<FeedbackResponseDto>>> Handle(GetByRatingQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Validate rating
                if (query.Rating < 1 || query.Rating > 5)
                {
                    return await Result<IEnumerable<FeedbackResponseDto>>.FaildAsync(false, "Rating must be between 1 and 5.");
                }

                var result = await _feedbackRepository.GetByRatingAsync(query.Rating);

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

                return await Result<IEnumerable<FeedbackResponseDto>>.SuccessAsync(responseDtos, $"Feedbacks with rating {query.Rating} retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<FeedbackResponseDto>>.FaildAsync(false, $"Error retrieving feedbacks by rating: {ex.Message}");
            }
        }
    }
} 