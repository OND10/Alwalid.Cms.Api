using ChatApp.Application.DTOs;
using MediatR;

namespace ChatApp.Application.Messages.Commands.CreateMessage;

/// <summary>
/// Command to create a new message
/// </summary>
public class CreateMessageCommand : IRequest<MessageDto>
{
    /// <summary>
    /// Gets or sets the message content
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user identifier who is sending the message
    /// </summary>
    public Guid UserId { get; set; }
}