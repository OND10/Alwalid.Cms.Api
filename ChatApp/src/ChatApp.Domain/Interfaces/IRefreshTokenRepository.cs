using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces;

/// <summary>
/// Repository interface for refresh token operations
/// </summary>
public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    /// <summary>
    /// Gets an active refresh token by its token value
    /// </summary>
    /// <param name="token">The refresh token value</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The refresh token if found and active, otherwise null</returns>
    Task<RefreshToken?> GetActiveTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active refresh tokens for a user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of active refresh tokens</returns>
    Task<IEnumerable<RefreshToken>> GetActiveTokensByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes all refresh tokens for a user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="reason">The reason for revoking tokens</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of tokens revoked</returns>
    Task<int> RevokeAllUserTokensAsync(Guid userId, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes expired refresh tokens
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of tokens removed</returns>
    Task<int> RemoveExpiredTokensAsync(CancellationToken cancellationToken = default);
}