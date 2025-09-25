using ChatApp.Application.DTOs;
using MediatR;

namespace ChatApp.Application.Messages.Queries.GetLatestMessages;

/// <summary>
/// Query to get the latest messages
/// </summary>
public class GetLatestMessagesQuery : IRequest<IEnumerable<MessageDto>>
{
    /// <summary>
    /// Gets or sets the number of messages to retrieve
    /// </summary>
    public int Count { get; set; } = 50;
}