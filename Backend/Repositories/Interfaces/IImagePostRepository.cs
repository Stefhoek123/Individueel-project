using Models;

namespace Interface;

public interface IImagePostRepository
{
    IEnumerable<ImagePost> GetAllImagePosts();
    ImagePost GetImagePostById(Guid id);
    void CreateImagePost(ImagePost imagePost);
    void UpdateImagePost(ImagePost imagePost);
    void DeleteImagePostById(Guid id);
}