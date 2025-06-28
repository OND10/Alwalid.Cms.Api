using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Feedback;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Queries.GetByPosition
{
    public class GetByPositionQueryHandler : IQueryHandler<GetByPositionQuery, IEnumerable<FeedbackResponseDto>>
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public GetByPositionQueryHandler(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Result<IEnumerable<FeedbackResponseDto>>> Handle(GetByPositionQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query.Position))
                {
                    return await Result<IEnumerable<FeedbackResponseDto>>.FaildAsync(false, "Position cannot be empty.");
                }

                var result = await _feedbackRepository.GetByPositionAsync(query.Position);

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

                return await Result<IEnumerable<FeedbackResponseDto>>.SuccessAsync(responseDtos, $"Feedbacks for position '{query.Position}' retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<FeedbackResponseDto>>.FaildAsync(false, $"Error retrieving feedbacks by position: {ex.Message}");
            }
        }
    }
} 