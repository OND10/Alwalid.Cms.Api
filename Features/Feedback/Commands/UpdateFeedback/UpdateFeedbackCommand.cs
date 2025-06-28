using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Commands.UpdateFeedback
{
    public class UpdateFeedbackCommand : ICommand<FeedbackResponseDto>
    {
        public int Id { get; set; }
        public FeedbackRequestDto Request { get; set; } = new();
    }
} 