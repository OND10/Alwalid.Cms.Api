using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Feedback;

namespace Alwalid.Cms.Api.Features.Feedback.Commands.DeleteFeedback
{
    public class DeleteFeedbackCommandHandler : ICommandHandler<DeleteFeedbackCommand, bool>
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public DeleteFeedbackCommandHandler(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Result<bool>> Handle(DeleteFeedbackCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if feedback exists
                if (!await _feedbackRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Feedback not found.");
                }

                var result = await _feedbackRepository.SoftDeleteAsync(command.Id);

                if (result)
                {
                    return await Result<bool>.SuccessAsync(true, "Feedback deleted successfully.", true);
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to delete feedback.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error deleting feedback: {ex.Message}");
            }
        }
    }
} 