using System.ComponentModel.DataAnnotations;

namespace Domain;

public class PostDto
{
    public Guid Id { get; set; }

    [Required]
    public string TextContent { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;   
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; init; }
    public Guid FamilyId { get; init; } 

    public PostDto(Guid id, string textContent, string imageUrl, DateTime createAt, Guid userId, Guid familyId)
    {
        Id = id;
        TextContent = textContent;
        ImageUrl = imageUrl;
        CreatedAt = createAt;
        UserId = userId;
        FamilyId = familyId;
    }

    public PostDto() { }
}