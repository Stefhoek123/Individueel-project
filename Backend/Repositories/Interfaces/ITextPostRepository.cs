using Models;

namespace Interface;

public interface ITextPostRepository
{
    IEnumerable<TextPost> GetAllTextPosts();
    TextPost GetTextPostById(Guid id);
    void CreateTextPost(TextPost textPost);
    void UpdateTextPost(TextPost textPost);
    void DeleteTextPostById(Guid id);
}