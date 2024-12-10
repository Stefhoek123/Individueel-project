using Domain;

namespace DAL.Interfaces;

public interface IImagePostContainer
{
    IEnumerable<ImagePostDto> GetAllImagePosts();
    ImagePostDto GetImagePostById(Guid id);
    void CreateImagePost(ImagePostDto imagePost);
    void UpdateImagePost(ImagePostDto imagePost);
    void DeleteImagePostById(Guid id);
}