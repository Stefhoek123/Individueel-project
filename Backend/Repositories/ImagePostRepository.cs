using Interface;
using Models;

namespace Repositories;

public class ImagePostRepository : IImagePostRepository
{
    private readonly BackendDbContext _backendDbContext;

    public ImagePostRepository(BackendDbContext backendDbContext)
    {
        _backendDbContext = backendDbContext;
    }

    public IEnumerable<ImagePost> GetAllImagePosts()
    {
        return _backendDbContext.ImagePosts.ToList();
    }

    public ImagePost GetImagePostById(Guid id)
    {
        return _backendDbContext.ImagePosts.FirstOrDefault(u => u.Id == id)!;
    }

    public void CreateImagePost(ImagePost imagePost)
    {
        _backendDbContext.ImagePosts.Add(imagePost);
        _backendDbContext.SaveChanges();
    }

    public void UpdateImagePost(ImagePost imagePost)
    {
        _backendDbContext.ImagePosts.Update(imagePost);
        _backendDbContext.SaveChanges();
    }

    public void DeleteImagePostById(Guid id)
    {
        _backendDbContext.ImagePosts.Remove(GetImagePostById(id));
        _backendDbContext.SaveChanges();
    }
}