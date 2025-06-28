using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Feedback;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Commands.UpdateFeedback
{
    public class UpdateFeedbackCommandHandler : ICommandHandler<UpdateFeedbackCommand, FeedbackResponseDto>
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public UpdateFeedbackCommandHandler(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Result<FeedbackResponseDto>> Handle(UpdateFeedbackCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if feedback exists
                var existingFeedback = await _feedbackRepository.GetByIdAsync(command.Id);
                if (existingFeedback == null)
                {
                    return await Result<FeedbackResponseDto>.FaildAsync(false, "Feedback not found.");
                }

                // Validate rating
                if (command.Request.Rating < 1 || command.Request.Rating > 5)
                {
                    return await Result<FeedbackResponseDto>.FaildAsync(false, "Rating must be between 1 and 5.");
                }

                // Update feedback
                existingFeedback.ArabicName = command.Request.ArabicName;
                existingFeedback.EnglishName = command.Request.EnglishName;
                existingFeedback.PhoneNumber = command.Request.PhoneNumber;
                existingFeedback.ImageUrl = existingFeedback.ImageUrl;
                existingFeedback.Comment = command.Request.Comment;
                existingFeedback.Position = command.Request.Position;
                existingFeedback.Rating = command.Request.Rating;

                var updatedFeedback = await _feedbackRepository.UpdateAsync(existingFeedback);

                // Map to response DTO
                var responseDto = new FeedbackResponseDto
                {
                    Id = updatedFeedback.Id,
                    ArabicName = updatedFeedback.ArabicName,
                    EnglishName = updatedFeedback.EnglishName,
                    PhoneNumber = updatedFeedback.PhoneNumber,
                    ImageUrl = updatedFeedback.ImageUrl,
                    Comment = updatedFeedback.Comment,
                    Position = updatedFeedback.Position,
                    IsActive = updatedFeedback.IsActive,
                    Rating = updatedFeedback.Rating,
                    CreatedAt = updatedFeedback.CreatedAt,
                    LastModifiedAt = updatedFeedback.LastModifiedAt
                };

                return await Result<FeedbackResponseDto>.SuccessAsync(responseDto, "Feedback updated successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<FeedbackResponseDto>.FaildAsync(false, $"Error updating feedback: {ex.Message}");
            }
        }
    }
} 