using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Alwalid.Cms.Api.Hubs
{
    //[Authorize] // Secure the hub so only authenticated users can connect.
    public class ChatHub : Hub
    {
        // This method will be called by the client to join a specific conversation's "group".
        // This ensures users only receive messages for the conversation they are currently viewing.
        public async Task JoinConversationGroup(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        // This method allows the client to leave a group when they switch conversations.
        public async Task LeaveConversationGroup(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }

        // This method is called automatically when a client connects.
        // We can use it for logging or associating the user with their connection.
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.Name);
            // You could, for example, add the user to a group based on their user ID
            // to send them notifications across all their devices.
            if (userId != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            }
            await base.OnConnectedAsync();
        }
    }
}
