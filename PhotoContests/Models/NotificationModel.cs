namespace PhotoContests.Models
{
    public class NotificationModel
    {
        public int idNotification { get; set; }
        public string title { get; set; }
        public DateTime sendDate { get; set; }
        public bool status { get; set; }
        public string senderName { get; set; }
        public string profilePicture { get; set; }
        public string senderRole { get; set; }

    }
}
