using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Chat : BaseModel
    {
        public int ReactId { get; set; }
        public DateTime Date { get; set; }
        public string SenderName { get; set; }
        public int ChatContent { get; set; }

        [ForeignKey(nameof(UserId))]
        public Guid UserId { get; init; }

        public Chat(Guid id, int reactId, DateTime date, string senderName, int chatContent, Guid userId)
        {
            Id = id;
            ReactId = reactId;
            Date = date;
            SenderName = senderName;
            ChatContent = chatContent;
            UserId = userId;
        }
    }
}
