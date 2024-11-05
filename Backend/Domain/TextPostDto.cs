using System.ComponentModel.DataAnnotations;

namespace Domain;

public class TextPostDto
{
    public Guid Id { get; set; }

    [Required]
    public string TextContent { get; set; } = null!;

    public TextPostDto(Guid id, string textContent)
    {
        Id = id;
        TextContent = textContent;
    }

    public TextPostDto() { }
}