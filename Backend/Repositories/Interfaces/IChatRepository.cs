using Models;

namespace Interface;

public interface IChatRepository
{
    IEnumerable<Chat> GetAllChats();
    Chat GetChatById(Guid id);
    List<Chat> GetChatsById(Guid id);
    void CreateChat(Chat chat);
    void UpdateChat(Chat chat);
    void DeleteChatById(Guid id);

}