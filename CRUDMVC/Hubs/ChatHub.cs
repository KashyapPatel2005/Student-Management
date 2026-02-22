using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CRUDMVC.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "GlobalRoom");
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string message)
        {
            var username = Context.User.IsInRole("Admin")
                ? "Admin"
                : Context.User.Identity.Name;

            await Clients.Group("GlobalRoom")
                .SendAsync("ReceiveMessage", username, message);
        }
    }
}