using System.ComponentModel.DataAnnotations;

namespace Domain;

public class ChatDto
{
    public Guid Id { get; set; }

    [Required]
    public int ReactId { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string SenderName { get; set; } = null!;
    [Required]
    public int ChatContent { get; set; }

    public Guid UserId { get; init; }

    public ChatDto(Guid id, int reactId, DateTime date, string senderName, int chatContent, Guid userId)
    {
        Id = id;
        ReactId = reactId;
        Date = date;
        SenderName = senderName;
        ChatContent = chatContent;
        UserId = userId;
    }

    public ChatDto() { }
}