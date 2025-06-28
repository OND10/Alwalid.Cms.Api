using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Commands.AddFeedback
{
    public class AddFeedbackCommand : ICommand<FeedbackResponseDto>
    {
        public FeedbackRequestDto Request { get; set; } = new();
    }
} 