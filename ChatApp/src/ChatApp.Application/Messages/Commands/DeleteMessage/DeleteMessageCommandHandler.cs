using ChatApp.Domain.Exceptions;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.Messages.Commands.DeleteMessage;

/// <summary>
/// Handler for the DeleteMessageCommand
/// </summary>
public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the DeleteMessageCommandHandler class
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    public DeleteMessageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the DeleteMessageCommand
    /// </summary>
    /// <param name="request">The command request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the message was deleted successfully</returns>
    public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        // Get the message
        var message = await _unitOfWork.Messages.GetByIdAsync(request.MessageId, cancellationToken);
        if (message == null)
        {
            throw new EntityNotFoundException(nameof(Domain.Entities.Message), request.MessageId);
        }

        // Check if the user owns the message
        if (message.UserId != request.UserId)
        {
            throw new Domain.Exceptions.UnauthorizedAccessException("You can only delete your own messages.");
        }

        // Soft delete the message
        message.MarkAsDeleted();
        await _unitOfWork.Messages.UpdateAsync(message, cancellationToken);
        
        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}