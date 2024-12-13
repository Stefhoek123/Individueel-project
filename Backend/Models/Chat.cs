using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Chat : BaseModel
    {
        public Guid PostId { get; set; }
        public DateTime Date { get; set; }
        public string ChatContent { get; set; }
        public Guid ReactId { get; set; }
        public string SenderName { get; set; }

        [ForeignKey(nameof(UserId))]
        public Guid UserId { get; init; }

        public Chat(Guid id, Guid postId, DateTime date, string chatContent, Guid reactId, string senderName, Guid userId)
        {
            Id = id;
            PostId = postId;
            Date = date;
            ChatContent = chatContent;
            ReactId = reactId;
            SenderName = senderName;
            UserId = userId;
        }
    }
}
