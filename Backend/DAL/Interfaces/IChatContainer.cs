using Domain;
using Models;

namespace DAL.Interfaces;

public interface IChatContainer
{
    IEnumerable<ChatDto> GetAllChats();
    ChatDto GetChatById(Guid id);
    List<ChatDto> GetChatsById(Guid id);
    void CreateChat(ChatDto chat);
    void UpdateChat(ChatDto chat);
    void DeleteChatById(Guid id);
}