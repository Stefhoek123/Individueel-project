using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Post : BaseModel
{
    public string TextContent { get; set; }
    public string ImageUrl { get; set; }    

    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; init; }

    public Post(Guid id, string textContent, string imageUrl, Guid userId)
    {
        Id = id;
        TextContent = textContent;
        ImageUrl = imageUrl;
        UserId = userId;
    }
}