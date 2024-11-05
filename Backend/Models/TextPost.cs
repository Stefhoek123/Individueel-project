namespace Models;

public class TextPost
{
    public int TextPostId { get; set; }
    public string TextContent { get; set; }

    public User UserId { get; set; }

    public TextPost(int textPostId, string textContent, User userId)
    {
        TextPostId = textPostId;
        TextContent = textContent;
        UserId = userId;
    }
}