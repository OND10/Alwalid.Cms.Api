using ChatApp.Application.DTOs;
using MediatR;

namespace ChatApp.Application.Messages.Queries.GetMessages;

/// <summary>
/// Query to get paginated messages
/// </summary>
public class GetMessagesQuery : IRequest<PaginatedListDto<MessageDto>>
{
    /// <summary>
    /// Gets or sets the page number (1-based)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the optional user ID to filter messages by user
    /// </summary>
    public Guid? UserId { get; set; }
}