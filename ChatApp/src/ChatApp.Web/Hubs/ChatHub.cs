using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatApp.Web.Hubs;

/// <summary>
/// SignalR hub for real-time chat messaging
/// </summary>
[Authorize]
public class ChatHub : Hub
{
    /// <summary>
    /// Handles user connection to the chat hub
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        
        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userName))
        {
            // Add user to a group (optional - for future features like private rooms)
            await Groups.AddToGroupAsync(Context.ConnectionId, "ChatRoom");
            
            // Notify other users that someone joined
            await Clients.Others.SendAsync("UserConnected", new
            {
                UserId = userId,
                UserName = userName,
                Message = $"{userName} joined the chat"
            });
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Handles user disconnection from the chat hub
    /// </summary>
    /// <param name="exception">The exception that caused the disconnection, if any</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        
        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userName))
        {
            // Remove user from group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ChatRoom");
            
            // Notify other users that someone left
            await Clients.Others.SendAsync("UserDisconnected", new
            {
                UserId = userId,
                UserName = userName,
                Message = $"{userName} left the chat"
            });
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Sends a typing indicator to other users
    /// </summary>
    /// <param name="isTyping">Whether the user is currently typing</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task SendTypingIndicator(bool isTyping)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        
        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userName))
        {
            await Clients.Others.SendAsync("UserTyping", new
            {
                UserId = userId,
                UserName = userName,
                IsTyping = isTyping
            });
        }
    }

    /// <summary>
    /// Broadcasts a new message to all connected clients
    /// </summary>
    /// <param name="messageDto">The message data to broadcast</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task BroadcastMessage(object messageDto)
    {
        await Clients.All.SendAsync("MessageReceived", messageDto);
    }

    /// <summary>
    /// Notifies clients that a message has been deleted
    /// </summary>
    /// <param name="messageId">The ID of the deleted message</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task BroadcastMessageDeleted(Guid messageId)
    {
        await Clients.All.SendAsync("MessageDeleted", new { MessageId = messageId });
    }
}