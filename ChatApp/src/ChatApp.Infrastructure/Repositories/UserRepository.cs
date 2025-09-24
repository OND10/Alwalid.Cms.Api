using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for user-specific operations
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the UserRepository class
    /// </summary>
    /// <param name="context">The database context</param>
    public UserRepository(ChatDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(googleId))
            return null;

        return await _dbSet
            .FirstOrDefaultAsync(u => u.GoogleId == googleId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateLastActiveAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken);
        if (user == null)
            return false;

        user.LastActiveAt = DateTime.UtcNow;
        _dbSet.Update(user);
        return true;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetActiveUsersAsync(DateTime since, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.LastActiveAt >= since)
            .OrderByDescending(u => u.LastActiveAt)
            .ToListAsync(cancellationToken);
    }
}