namespace Models;

public class ImagePost
{
    public int ImagePostId { get; set; }
    public string Alt { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }

    public User UserId { get; set; }

    public ImagePost(int imagePostId, string alt, string url, string description, User userId)
    {
        ImagePostId = imagePostId;
        Alt = alt;
        Url = url;
        Description = description;
        UserId = userId;
    }
}