using Interface;
using Models;

namespace Repositories;

public class PostRepository : IPostRepository
{
    private readonly BackendDbContext _backendDbContext;

    public PostRepository(BackendDbContext backendDbContext)
    {
        _backendDbContext = backendDbContext;
    }

    public IEnumerable<Post> GetAllPosts()
    {
        return _backendDbContext.Posts.ToList();
    }

    public Post GetPostById(Guid id)
    {
        return _backendDbContext.Posts.FirstOrDefault(u => u.Id == id)!;
    }

    public List<Post> GetPostsByFamilyId(Guid id)
    {
        return _backendDbContext.Posts.Where(p => p.FamilyId == id).ToList();
    }

    public void CreatePost(Post post)
    {
        _backendDbContext.Posts.Add(post);
        _backendDbContext.SaveChanges();
    }

    public void UpdatePost(Post post)
    {
        _backendDbContext.Posts.Update(post);
        _backendDbContext.SaveChanges();
    }

    public void DeletePostById(Guid id)
    {
        _backendDbContext.Posts.Remove(GetPostById(id));
        _backendDbContext.SaveChanges();
    }
}