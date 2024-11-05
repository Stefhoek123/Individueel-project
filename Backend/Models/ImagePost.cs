using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class ImagePost : BaseModel
{
    public string Alt { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }

    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; init; }

    public ImagePost(Guid id, string alt, string url, string description, Guid userId)
    {
        Id = id;
        Alt = alt;
        Url = url;
        Description = description;
        UserId = userId;
    }
}