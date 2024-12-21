using Domain;
using Models;

namespace DAL.Interfaces;

public interface IChatContainer
{
    IEnumerable<ChatDto> GetAllChats();
    List<ChatDto> GetChatById(Guid id);
    void CreateChat(ChatDto chat);
    void UpdateChat(ChatDto chat);
    void DeleteChatById(Guid id);
}