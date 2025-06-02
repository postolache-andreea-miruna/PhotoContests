using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Entities
{
    public class Notification
    {
        [Key]
        public int idNotification { get; set; }
        
        public string idUser2 { get; set; }//receiver
        public string message { get; set; }
        public string title { get; set; }
        public DateTime sendDate { get; set; }
        public bool status { get; set; }

        [ForeignKey("User")]
        public string idUser { get; set; }

        public User User { get; set; }
       


    }
}
