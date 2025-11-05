using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for message-specific operations
/// </summary>
public class MessageRepository : Repository<Message>, IMessageRepository
{
    /// <summary>
    /// Initializes a new instance of the MessageRepository class
    /// </summary>
    /// <param name="context">The database context</param>
    public MessageRepository(ChatDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Message> Messages, int TotalCount)> GetMessagesAsync(
        int pageNumber, 
        int pageSize, 
        Guid? userId = null, 
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Limit max page size

        var query = _dbSet
            .Include(m => m.User)
            .AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(m => m.UserId == userId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var messages = await query
            .OrderByDescending(m => m.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (messages, totalCount);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Message>> GetLatestMessagesAsync(int count = 50, CancellationToken cancellationToken = default)
    {
        if (count < 1) count = 1;
        if (count > 100) count = 100; // Limit max count

        return await _dbSet
            .Include(m => m.User)
            .OrderByDescending(m => m.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Message> Messages, int TotalCount)> GetMessagesByUserAsync(
        Guid userId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Limit max page size

        var query = _dbSet
            .Include(m => m.User)
            .Where(m => m.UserId == userId);

        var totalCount = await query.CountAsync(cancellationToken);

        var messages = await query
            .OrderByDescending(m => m.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (messages, totalCount);
    }

    /// <inheritdoc />
    public override async Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }
}