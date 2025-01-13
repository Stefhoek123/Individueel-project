using Domain;

namespace DAL.Interfaces;

public interface IPostContainer
{
    IEnumerable<PostDto> GetAllPosts();
    PostDto GetPostById(Guid id);
    List<PostDto> GetPostsByFamilyId(Guid id);
    void CreatePost(PostDto post);
    void UpdatePost(PostDto post);
    void DeletePostById(Guid id);
}