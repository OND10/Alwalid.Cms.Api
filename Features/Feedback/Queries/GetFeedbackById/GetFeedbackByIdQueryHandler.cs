using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Feedback;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Queries.GetFeedbackById
{
    public class GetFeedbackByIdQueryHandler : IQueryHandler<GetFeedbackByIdQuery, FeedbackResponseDto>
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public GetFeedbackByIdQueryHandler(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Result<FeedbackResponseDto>> Handle(GetFeedbackByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var feedback = await _feedbackRepository.GetByIdAsync(query.Id);

                if (feedback == null)
                {
                    return await Result<FeedbackResponseDto>.FaildAsync(false, "Feedback not found.");
                }

                var responseDto = new FeedbackResponseDto
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
                };

                return await Result<FeedbackResponseDto>.SuccessAsync(responseDto, "Feedback retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<FeedbackResponseDto>.FaildAsync(false, $"Error retrieving feedback: {ex.Message}");
            }
        }
    }
} 