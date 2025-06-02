using System.ComponentModel.DataAnnotations;

namespace PhotoContests.Entities
{
    public class Review
    {
        [Key]
        public int idReview { get; set; }
        public string idUser { get; set; }
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }


        public string message { get; set; }
        public DateTime sendDate { get; set; }


        public Juror Juror { get; set; }
       /* public Photo Photo { get; set; }
        public Competition Competition { get; set; }
        public Section Section { get; set; }*/
        public Participation Participation { get; set; }
    }
}
