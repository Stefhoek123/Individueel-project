namespace Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public int ReactId { get; set; }
        public DateTime Date { get; set; }
        public string SenderName { get; set; }
        public int ChatContent { get; set; }

        public User UserId { get; set; }
        public User FirstName { get; set; }

        public Chat(int chatId, int reactId, DateTime date, string senderName, int chatContent, User userId, User firstName)
        { 
            ChatId = chatId;
            ReactId = reactId;
            Date = date;
            SenderName = senderName;
            ChatContent = chatContent;
            UserId = userId;
            FirstName = firstName;
        }
    }
}
