using Domain;

namespace DAL.Interfaces;

public interface IChatContainer
{
    IEnumerable<ChatDto> GetAllChats();
    ChatDto GetChatById(Guid id);
    void CreateChat(ChatDto chat);
    void UpdateChat(ChatDto chat);
    void DeleteChatById(Guid id);
}