using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for refresh token operations
/// </summary>
public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    /// <summary>
    /// Initializes a new instance of the RefreshTokenRepository class
    /// </summary>
    /// <param name="context">The database context</param>
    public RefreshTokenRepository(ChatDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<RefreshToken?> GetActiveTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        return await _dbSet
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow, 
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(rt => rt.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> RevokeAllUserTokensAsync(Guid userId, string reason, CancellationToken cancellationToken = default)
    {
        var activeTokens = await _dbSet
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.Revoke(reason);
        }

        _dbSet.UpdateRange(activeTokens);
        return activeTokens.Count;
    }

    /// <inheritdoc />
    public async Task<int> RemoveExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        var expiredTokens = await _dbSet
            .Where(rt => rt.ExpiresAt <= DateTime.UtcNow || rt.IsRevoked)
            .ToListAsync(cancellationToken);

        foreach (var token in expiredTokens)
        {
            token.MarkAsDeleted();
        }

        _dbSet.UpdateRange(expiredTokens);
        return expiredTokens.Count;
    }
}