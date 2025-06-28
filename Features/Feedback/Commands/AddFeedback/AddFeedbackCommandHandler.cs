using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Common.Helper.Interface;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Feedback;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Commands.AddFeedback
{
    public class AddFeedbackCommandHandler : ICommandHandler<AddFeedbackCommand, FeedbackResponseDto>
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IImageRepository _imageRepository;

        public AddFeedbackCommandHandler(IFeedbackRepository feedbackRepository, IImageRepository imageRepository)
        {
            _feedbackRepository = feedbackRepository;
            _imageRepository = imageRepository;
        }

        public async Task<Result<FeedbackResponseDto>> Handle(AddFeedbackCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Validate rating
                if (command.Request.Rating < 1 || command.Request.Rating > 5)
                {
                    return await Result<FeedbackResponseDto>.FaildAsync(false, "Rating must be between 1 and 5.");
                }

                // Create new feedback
                var feedback = new Entities.Feedback
                {
                    ArabicName = command.Request.ArabicName,
                    EnglishName = command.Request.EnglishName,
                    PhoneNumber = command.Request.PhoneNumber,
                    Comment = command.Request.Comment,
                    Position = command.Request.Position,
                    Rating = command.Request.Rating
                };

                if (command.Request.Image != null)
                {
                    var imageUrl = await _imageRepository.Upload(feedback, command.Request.Image);

                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        feedback.ImageUrl = imageUrl;
                    }
                }

                var createdFeedback = await _feedbackRepository.CreateAsync(feedback);

                // Map to response DTO
                var responseDto = new FeedbackResponseDto
                {
                    Id = createdFeedback.Id,
                    ArabicName = createdFeedback.ArabicName,
                    EnglishName = createdFeedback.EnglishName,
                    PhoneNumber = createdFeedback.PhoneNumber,
                    ImageUrl = createdFeedback.ImageUrl,
                    Comment = createdFeedback.Comment,
                    Position = createdFeedback.Position,
                    IsActive = createdFeedback.IsActive,
                    Rating = createdFeedback.Rating,
                    CreatedAt = createdFeedback.CreatedAt,
                    LastModifiedAt = createdFeedback.LastModifiedAt
                };

                return await Result<FeedbackResponseDto>.SuccessAsync(responseDto, "Feedback submitted successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<FeedbackResponseDto>.FaildAsync(false, $"Error submitting feedback: {ex.Message}");
            }
        }
    }
} 