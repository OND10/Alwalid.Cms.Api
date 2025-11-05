using ChatApp.Application.DTOs;
using ChatApp.Application.Messages.Commands.CreateMessage;
using ChatApp.Application.Messages.Commands.DeleteMessage;
using ChatApp.Application.Messages.Queries.GetMessages;
using ChatApp.Application.Messages.Queries.GetLatestMessages;
using ChatApp.Web.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatApp.Web.Controllers;

/// <summary>
/// Controller for message operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<MessagesController> _logger;

    /// <summary>
    /// Initializes a new instance of the MessagesController class
    /// </summary>
    /// <param name="mediator">The MediatR instance</param>
    /// <param name="hubContext">The SignalR hub context</param>
    /// <param name="logger">The logger</param>
    public MessagesController(IMediator mediator, IHubContext<ChatHub> hubContext, ILogger<MessagesController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets paginated messages with optional user filtering
    /// </summary>
    /// <param name="pageNumber">The page number (1-based)</param>
    /// <param name="pageSize">The page size (max 100)</param>
    /// <param name="userId">Optional user ID to filter messages</param>
    /// <returns>Paginated list of messages</returns>
    [HttpGet]
    public async Task<ActionResult<PaginatedListDto<MessageDto>>> GetMessages(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? userId = null)
    {
        try
        {
            var query = new GetMessagesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                UserId = userId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving messages");
            return StatusCode(500, new { error = "Failed to retrieve messages" });
        }
    }

    /// <summary>
    /// Gets the latest messages for initial chat loading
    /// </summary>
    /// <param name="count">Number of messages to retrieve (max 100)</param>
    /// <returns>Collection of latest messages</returns>
    [HttpGet("latest")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetLatestMessages([FromQuery] int count = 50)
    {
        try
        {
            var query = new GetLatestMessagesQuery { Count = count };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving latest messages");
            return StatusCode(500, new { error = "Failed to retrieve latest messages" });
        }
    }

    /// <summary>
    /// Creates a new message and broadcasts it to all connected clients
    /// </summary>
    /// <param name="request">The create message request</param>
    /// <returns>The created message</returns>
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage([FromBody] CreateMessageRequest request)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var command = new CreateMessageCommand
            {
                Content = request.Content,
                UserId = currentUserId.Value
            };

            var result = await _mediator.Send(command);

            // Broadcast the new message to all connected clients
            await _hubContext.Clients.All.SendAsync("MessageReceived", result);

            return CreatedAtAction(nameof(GetMessages), new { }, result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid message creation request");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating message");
            return StatusCode(500, new { error = "Failed to create message" });
        }
    }

    /// <summary>
    /// Deletes a message (only owner can delete)
    /// </summary>
    /// <param name="id">The message ID</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(Guid id)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var command = new DeleteMessageCommand
            {
                MessageId = id,
                UserId = currentUserId.Value
            };

            var success = await _mediator.Send(command);

            if (success)
            {
                // Broadcast the message deletion to all connected clients
                await _hubContext.Clients.All.SendAsync("MessageDeleted", new { MessageId = id });
                return NoContent();
            }

            return NotFound();
        }
        catch (Domain.Exceptions.EntityNotFoundException ex)
        {
            _logger.LogWarning(ex, "Message not found for deletion: {MessageId}", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Domain.Exceptions.UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized message deletion attempt: {MessageId}", id);
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting message: {MessageId}", id);
            return StatusCode(500, new { error = "Failed to delete message" });
        }
    }

    /// <summary>
    /// Gets the current user ID from the JWT claims
    /// </summary>
    /// <returns>The current user ID or null if not authenticated</returns>
    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}

/// <summary>
/// Request model for creating a message
/// </summary>
public class CreateMessageRequest
{
    /// <summary>
    /// Gets or sets the message content
    /// </summary>
    public string Content { get; set; } = string.Empty;
}