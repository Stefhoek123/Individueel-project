using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace Backend.Hubs
{
    public sealed class ChatHub : Hub<IChatClient>
    {
        private static readonly Dictionary<string, string> ConnectedUsers = new();
        private static ConcurrentBag<Chat> _chats = new();

        public override async Task OnConnectedAsync()
        {
            string? userEmail = Context.GetHttpContext()?.Request.Query["Email"];
            ConnectedUsers[Context.ConnectionId] = userEmail ?? "Anonymous";
            await Clients.All.ReceiveMessage(new Chat(new Guid(), new Guid(), DateTime.Now, $"{userEmail} joined the chat", new Guid(), "System", new Guid()));
        }

        public async Task SendMessage(Chat message)
        {
            _chats.Add(message);    

            message.Date = DateTime.Now;
            await Clients.All.ReceiveMessage(message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (ConnectedUsers.TryGetValue(Context.ConnectionId, out string userEmail))
            {
                ConnectedUsers.Remove(Context.ConnectionId);
                await Clients.All.ReceiveMessage(new Chat(new Guid(), new Guid(), DateTime.Now, $"{userEmail} has left the chat", new Guid(), "System", new Guid()));
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
