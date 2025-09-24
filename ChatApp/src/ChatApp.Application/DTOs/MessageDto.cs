using ChatApp.Application.Common.Mappings;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.DTOs;

/// <summary>
/// Data transfer object for message information
/// </summary>
public class MessageDto : IMapFrom<Message>
{
    /// <summary>
    /// Gets or sets the message identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the message content
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user identifier who sent the message
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the user who sent the message
    /// </summary>
    public UserDto User { get; set; } = null!;

    /// <summary>
    /// Gets or sets the timestamp when the message was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the message was edited
    /// </summary>
    public DateTime? EditedAt { get; set; }

    /// <summary>
    /// Gets a value indicating whether the message has been edited
    /// </summary>
    public bool IsEdited => EditedAt.HasValue;
}