namespace Backend;

public interface IChatClient
{
    Task ReceiveMessage(string message);    
}