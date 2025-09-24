using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

/// <summary>
/// Represents a refresh token for user authentication
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>
    /// Gets or sets the token value
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the refresh token
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets whether the token has been revoked
    /// </summary>
    public bool IsRevoked { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the token was revoked
    /// </summary>
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Gets or sets the reason for revoking the token
    /// </summary>
    public string? RevokedReason { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the user who owns this refresh token
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property for the user who owns this refresh token
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Gets a value indicating whether the refresh token is active
    /// </summary>
    public bool IsActive => !IsRevoked && !IsExpired && !IsDeleted;

    /// <summary>
    /// Gets a value indicating whether the refresh token is expired
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    /// <summary>
    /// Revokes the refresh token with an optional reason
    /// </summary>
    /// <param name="reason">The reason for revoking the token</param>
    public void Revoke(string? reason = null)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        RevokedReason = reason;
    }
}