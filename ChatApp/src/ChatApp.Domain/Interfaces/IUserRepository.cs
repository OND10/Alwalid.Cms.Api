using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces;

/// <summary>
/// Repository interface for user-specific operations
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Gets a user by their email address
    /// </summary>
    /// <param name="email">The user's email address</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, otherwise null</returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their Google OAuth identifier
    /// </summary>
    /// <param name="googleId">The Google OAuth subject identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, otherwise null</returns>
    Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the user's last active timestamp
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if updated successfully, otherwise false</returns>
    Task<bool> UpdateLastActiveAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets users who were active within the specified time frame
    /// </summary>
    /// <param name="since">The timestamp to check activity since</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of active users</returns>
    Task<IEnumerable<User>> GetActiveUsersAsync(DateTime since, CancellationToken cancellationToken = default);
}