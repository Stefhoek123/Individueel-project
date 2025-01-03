using Models;

namespace Backend.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(Chat message);
}