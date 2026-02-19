using Microsoft.AspNetCore.SignalR;

namespace Filminurk.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            var user = Context.User?.Identity?.Name ?? "Unknown User";

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
