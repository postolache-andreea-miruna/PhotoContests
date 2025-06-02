using PhotoContests.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Models
{
    public class NotificationCreateModel
    {
        public string emailReceiver {get; set; }
        public string message { get; set; }
        public string title { get; set; }
        public string emailSender { get; set; }
    }
}
