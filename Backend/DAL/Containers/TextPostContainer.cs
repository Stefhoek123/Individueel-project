using DAL.Interfaces;
using Domain;
using Interface;

namespace DAL.Containers;

public class TextPostContainer : ITextPostContainer
{
    private readonly ITextPostRepository _textPostRepository;

    public TextPostContainer(ITextPostRepository textPostRepository)
    {
        _textPostRepository = textPostRepository;
    }

    public IEnumerable<TextPostDto> GetAllTextPosts()
    {
        return _textPostRepository.GetAllTextPosts()
            .Select(Mappers.TextPostMapper.ToDto);
    }

    public TextPostDto GetTextPostById(Guid id)
    {
        return Mappers.TextPostMapper.ToDto(_textPostRepository.GetTextPostById(id));
    }

    public void CreateTextPost(TextPostDto textPost)
    {
        var textPostMapper = Mappers.TextPostMapper.ToModel(textPost);
        textPostMapper.Id = Guid.NewGuid();
        _textPostRepository.CreateTextPost(textPostMapper);
    }

    public void UpdateTextPost(TextPostDto textPost)
    {
        _textPostRepository.UpdateTextPost(Mappers.TextPostMapper.ToModel(textPost));
    }

    public void DeleteTextPostById(Guid id)
    {
        _textPostRepository.DeleteTextPostById(id);
    }
}