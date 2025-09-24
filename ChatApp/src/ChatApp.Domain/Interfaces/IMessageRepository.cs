using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces;

/// <summary>
/// Repository interface for message-specific operations
/// </summary>
public interface IMessageRepository : IRepository<Message>
{
    /// <summary>
    /// Gets messages with pagination support
    /// </summary>
    /// <param name="pageNumber">The page number (1-based)</param>
    /// <param name="pageSize">The number of messages per page</param>
    /// <param name="userId">Optional user ID to filter messages by user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of messages with user information</returns>
    Task<(IEnumerable<Message> Messages, int TotalCount)> GetMessagesAsync(
        int pageNumber, 
        int pageSize, 
        Guid? userId = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest messages with user information
    /// </summary>
    /// <param name="count">Number of messages to retrieve</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of messages with user information</returns>
    Task<IEnumerable<Message>> GetLatestMessagesAsync(int count = 50, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets messages by user with pagination
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="pageNumber">The page number (1-based)</param>
    /// <param name="pageSize">The number of messages per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of user messages</returns>
    Task<(IEnumerable<Message> Messages, int TotalCount)> GetMessagesByUserAsync(
        Guid userId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default);
}