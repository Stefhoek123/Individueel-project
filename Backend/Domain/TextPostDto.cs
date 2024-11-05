using System.ComponentModel.DataAnnotations;

namespace Domain;

public class TextPostDto
{
    public Guid Id { get; set; }

    [Required]
    public string TextContent { get; set; } = null!;

    public Guid UserId { get; init; }

    public TextPostDto(Guid id, string textContent, Guid userId)
    {
        Id = id;
        TextContent = textContent;
        UserId = userId;
    }

    public TextPostDto() { }
}