using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Feedback.Commands.DeleteFeedback
{
    public class DeleteFeedbackCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
} 