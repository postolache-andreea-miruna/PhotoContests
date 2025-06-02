using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Entities
{
    public class Message
    {
        [Key]
        public int idMessage { get; set; }

        public string messageText { get; set; }
        public DateTime sendDate { get; set; }
        public bool visibility { get; set; }

        [ForeignKey("User")]
        public string idUser { get; set; } //from
        public string idUser2 { get; set; } //to

        public User User { get; set; }
    }
}
