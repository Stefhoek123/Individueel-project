using Domain;

namespace DAL.Interfaces;

public interface ITextPostContainer
{
    IEnumerable<TextPostDto> GetAllTextPosts();
    TextPostDto GetTextPostById(Guid id);
    void CreateTextPost(TextPostDto textPost);
    void UpdateTextPost(TextPostDto textPost);
    void DeleteTextPostById(Guid id);
}