using System.ComponentModel.DataAnnotations;

namespace PhotoContests.Entities
{
    public class Report
    {
        [Key]
        public int idReport { get; set; }

        public string idUser { get; set; }
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }


        public string message { get; set; }
        public int noAccept { get; set; } = 0;
        public int noDecline { get; set; } = 0;

        public User User { get; set; }
        public Participation Participation { get; set; }
    }
}
