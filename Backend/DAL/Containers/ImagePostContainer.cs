using DAL.Interfaces;
using Domain;
using Interface;
using Models;
using Repositories;

namespace DAL.Containers;

public class ImagePostContainer : IImagePostContainer
{
    private readonly IImagePostRepository _imagePostRepository;

    public ImagePostContainer(IImagePostRepository imagePostRepository)
    {
        _imagePostRepository = imagePostRepository;
    }

    public IEnumerable<ImagePostDto> GetAllImagePosts()
    {
        return _imagePostRepository.GetAllImagePosts()
            .Select(Mappers.ImagePostMapper.ToDto);
    }

    public ImagePostDto GetImagePostById(Guid id)
    {
        return Mappers.ImagePostMapper.ToDto(_imagePostRepository.GetImagePostById(id));
    }

    public void CreateImagePost(ImagePostDto imagePost)
    {
        var imagePostMapper = Mappers.ImagePostMapper.ToModel(imagePost);
        imagePostMapper.Id = Guid.NewGuid();
        _imagePostRepository.CreateImagePost(imagePostMapper);
    }

    public void UpdateImagePost(ImagePostDto imagePost)
    {
        _imagePostRepository.UpdateImagePost(Mappers.ImagePostMapper.ToModel(imagePost));
    }

    public void DeleteImagePostById(Guid id)
    {
        _imagePostRepository.DeleteImagePostById(id);
    }
}