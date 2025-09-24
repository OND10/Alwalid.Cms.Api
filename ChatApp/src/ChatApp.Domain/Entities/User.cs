using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents a user in the chat application
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Gets or sets the user's email address
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the user's display name
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the Google OAuth subject identifier
    /// </summary>
    public required string GoogleId { get; set; }

    /// <summary>
    /// Gets or sets the user's profile picture URL from Google
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the user was last active
    /// </summary>
    public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property for the messages sent by this user
    /// </summary>
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    /// <summary>
    /// Navigation property for the refresh tokens associated with this user
    /// </summary>
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}