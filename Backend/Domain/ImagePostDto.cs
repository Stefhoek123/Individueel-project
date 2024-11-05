using System.ComponentModel.DataAnnotations;

namespace Domain;

public class ImagePostDto
{
    public Guid Id { get; set; }

    [Required]
    public string Alt { get; set; } = null!;
    [Required]
    public string Url { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;

    public ImagePostDto(Guid id, string alt, string url, string description)
    {
        Id = id;
        Alt = alt;
        Url = url;
        Description = description;
    }

    public ImagePostDto() { }
}