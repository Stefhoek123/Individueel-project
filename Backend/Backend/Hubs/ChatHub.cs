using System;
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
            await Clients.All.ReceiveMessage(new Chat(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, $"{userEmail} joined the chat", Guid.NewGuid(), "System", Guid.NewGuid()));
        }

        public async Task SendMessage(Chat message)
        {
            _chats.Add(message);    

            message.Date = DateTime.Now;
            await Clients.All.ReceiveMessage(message);
        }

        public async Task DeleteMessage(Guid id)
        {
            Chat? chat = _chats.FirstOrDefault(c => c.ReactId == id);
            if (chat != null)
            {
                _chats.TryTake(out chat);
                await Clients.All.DeleteMessage(id);
            }
        }   

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (ConnectedUsers.TryGetValue(Context.ConnectionId, out string? userEmail))
            {
                ConnectedUsers.Remove(Context.ConnectionId);
                await Clients.All.ReceiveMessage(new Chat(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, $"{userEmail} has left the chat", Guid.NewGuid(), "System", Guid.NewGuid()));
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
