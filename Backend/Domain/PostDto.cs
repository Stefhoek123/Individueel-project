using System.ComponentModel.DataAnnotations;

namespace Domain;

public class PostDto
{
    public Guid Id { get; set; }

    [Required]
    public string TextContent { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;   

    public Guid UserId { get; init; }

    public PostDto(Guid id, string textContent, string imageUrl, Guid userId)
    {
        Id = id;
        TextContent = textContent;
        ImageUrl = imageUrl;
        UserId = userId;
    }

    public PostDto() { }
}