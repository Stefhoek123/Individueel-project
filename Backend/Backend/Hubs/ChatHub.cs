using Microsoft.AspNetCore.SignalR;
using Models;

namespace Backend.Hubs
{
    public sealed class ChatHub : Hub<IChatClient>
    {
        private static readonly Dictionary<string, string> ConnectedUsers = new();

        public override async Task OnConnectedAsync()
        {
            string? username = Context.GetHttpContext()?.Request.Query["username"];
            ConnectedUsers[Context.ConnectionId] = username ?? "Anonymous";
            await Clients.All.ReceiveMessage(new Chat(new Guid(), new Guid(), DateTime.Now, $"{username} joined the chat", new Guid(), "System", new Guid()));
        }

        public async Task SendMessage(Chat message)
        {
            message.Date = DateTime.Now;
            await Clients.All.ReceiveMessage(message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (ConnectedUsers.TryGetValue(Context.ConnectionId, out string username))
            {
                ConnectedUsers.Remove(Context.ConnectionId);
                await Clients.All.ReceiveMessage(new Chat(new Guid(), new Guid(), DateTime.Now, $"{username} has left the chat", new Guid(), "System", new Guid()));
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
