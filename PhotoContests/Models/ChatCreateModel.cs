namespace PhotoContests.Models
{
    public class ChatCreateModel
    {
        public string idUser { get; set; } //sender
        public string idUser2 { get; set; } //receiver
        public string messageText { get; set; }
        public bool visibility { get; set; }
    }
}
