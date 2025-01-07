using Interface;
using Models;

namespace Repositories;

public class ChatRepository : IChatRepository
{
    private readonly BackendDbContext _backendDbContext;

    public ChatRepository(BackendDbContext backendDbContext)
    {
        _backendDbContext = backendDbContext;
    }

    public IEnumerable<Chat> GetAllChats()
    {
        return _backendDbContext.Chats.ToList();
    }

    public Chat GetChatById(Guid id)
    {
        return _backendDbContext.Chats.FirstOrDefault(u => u.ReactId == id)!;
    }

    public List<Chat> GetChatsById(Guid id)
    {
        return _backendDbContext.Chats.Where(c => c.PostId == id).ToList();
    }

    public void CreateChat(Chat chat)
    {
        _backendDbContext.Chats.Add(chat);
        _backendDbContext.SaveChanges();
    }

    public void UpdateChat(Chat chat)
    {
        _backendDbContext.Chats.Update(chat);
        _backendDbContext.SaveChanges();
    }

    public void DeleteChatById(Guid id)
    {
        _backendDbContext.Chats.Remove(GetChatById(id));
        _backendDbContext.SaveChanges();
    }
}