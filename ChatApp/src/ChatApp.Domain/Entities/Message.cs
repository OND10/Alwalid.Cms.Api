using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents a chat message in the application
/// </summary>
public class Message : BaseEntity
{
    /// <summary>
    /// Gets or sets the content of the message
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the user who sent the message
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property for the user who sent the message
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Gets or sets the timestamp when the message was last edited
    /// </summary>
    public DateTime? EditedAt { get; set; }

    /// <summary>
    /// Gets a value indicating whether the message has been edited
    /// </summary>
    public bool IsEdited => EditedAt.HasValue;

    /// <summary>
    /// Marks the message as edited and updates the EditedAt timestamp
    /// </summary>
    public void MarkAsEdited()
    {
        EditedAt = DateTime.UtcNow;
    }
}