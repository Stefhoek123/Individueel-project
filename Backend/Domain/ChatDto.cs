using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class ChatDto
{
    public Guid Id { get; set; }
    [Required]
    public Guid PostId { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string? ChatContent { get; set; }
    [Required]
    public Guid ReactId { get; set; }
    [Required]
    public string SenderName { get; set; }

    [Required]
    public Guid UserId { get; init; }

    public ChatDto(Guid id, Guid postId, DateTime date, string chatContent, Guid reactId, string senderName, Guid userId)
    {
        Id = id;
        PostId = postId;
        Date = date;
        ChatContent = chatContent;
        ReactId = reactId;
        SenderName = senderName;
        UserId = userId;
    }

    public ChatDto() { }
}