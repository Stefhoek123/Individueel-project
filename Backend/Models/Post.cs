using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Post : BaseModel
{
    public string TextContent { get; set; }
    public string ImageUrl { get; set; }    
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; init; }

    [ForeignKey(nameof(FamilyId))]
    public Guid FamilyId { get; init; }

    public Post(Guid id, string textContent, string imageUrl, DateTime createdAt, Guid userId, Guid familyId)
    {
        Id = id;
        TextContent = textContent;
        ImageUrl = imageUrl;
        CreatedAt = createdAt;
        UserId = userId;
        FamilyId = familyId;
    }
}