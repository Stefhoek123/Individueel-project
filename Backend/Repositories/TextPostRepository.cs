using Interface;
using Models;

namespace Repositories;

public class TextPostRepository : ITextPostRepository
{
    private readonly BackendDbContext _backendDbContext;

    public TextPostRepository(BackendDbContext backendDbContext)
    {
        _backendDbContext = backendDbContext;
    }

    public IEnumerable<TextPost> GetAllTextPosts()
    {
        return _backendDbContext.TextPosts.ToList();
    }

    public TextPost GetTextPostById(Guid id)
    {
        return _backendDbContext.TextPosts.FirstOrDefault(u => u.Id == id)!;
    }

    public void CreateTextPost(TextPost textPost)
    {
        _backendDbContext.TextPosts.Add(textPost);
        _backendDbContext.SaveChanges();
    }

    public void UpdateTextPost(TextPost textPost)
    {
        _backendDbContext.TextPosts.Update(textPost);
        _backendDbContext.SaveChanges();
    }

    public void DeleteTextPostById(Guid id)
    {
        _backendDbContext.TextPosts.Remove(GetTextPostById(id));
        _backendDbContext.SaveChanges();
    }
}