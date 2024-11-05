using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class TextPost : BaseModel
{
    public string TextContent { get; set; }

    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; init; }

    public TextPost(Guid id, string textContent, Guid userId)
    {
        Id = id;
        TextContent = textContent;
        UserId = userId;
    }
}