using MediatR;

namespace ChatApp.Application.Messages.Commands.DeleteMessage;

/// <summary>
/// Command to delete a message
/// </summary>
public class DeleteMessageCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the message identifier
    /// </summary>
    public Guid MessageId { get; set; }

    /// <summary>
    /// Gets or sets the user identifier who is requesting the deletion
    /// </summary>
    public Guid UserId { get; set; }
}