using Models;

namespace Interface;

public interface IPostRepository
{
    IEnumerable<Post> GetAllPosts();
    Post GetPostById(Guid id);
    List<Post> GetPostsByFamilyId(Guid id);
    void CreatePost(Post post);
    void UpdatePost(Post post);
    void DeletePostById(Guid id);
}