using ChatApp.Application.Common.Mappings;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.DTOs;

/// <summary>
/// Data transfer object for user information
/// </summary>
public class UserDto : IMapFrom<User>
{
    /// <summary>
    /// Gets or sets the user identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's display name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's profile picture URL
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the user was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the user was last active
    /// </summary>
    public DateTime LastActiveAt { get; set; }
}