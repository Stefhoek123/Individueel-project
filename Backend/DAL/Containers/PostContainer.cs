using DAL.Interfaces;
using Domain;
using Interface;
using Repositories;

namespace DAL.Containers;

public class PostContainer : IPostContainer
{
    private readonly IPostRepository _postRepository;

    public PostContainer(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public IEnumerable<PostDto> GetAllPosts()
    {
        return _postRepository.GetAllPosts()
            .Select(Mappers.PostMapper.ToDto);
    }

    public PostDto GetPostById(Guid id)
    {
        return Mappers.PostMapper.ToDto(_postRepository.GetPostById(id));
    }

    public IEnumerable<PostDto> GetPostsByFamilyId(Guid id)
    {
        var posts = _postRepository.GetPostsByFamilyId(id);

        return posts.Select(p => Mappers.PostMapper.ToDto(p)).ToList();
    }


    public void CreatePost(PostDto post)
    {
        var postMapper = Mappers.PostMapper.ToModel(post);
        postMapper.Id = Guid.NewGuid();
        _postRepository.CreatePost(postMapper);
    }

    public void UpdatePost(PostDto post)
    {
        _postRepository.UpdatePost(Mappers.PostMapper.ToModel(post));
    }

    public void DeletePostById(Guid id)
    {
        _postRepository.DeletePostById(id);
    }
}