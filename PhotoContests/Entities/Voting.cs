using System.ComponentModel.DataAnnotations;

namespace PhotoContests.Entities
{
    public class Voting
    {
        [Key]
        public int idVoting { get; set; }
        public string idUser { get; set; }
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }
        

        public int publicVote { get; set; }
        public int jurorVote { get; set; }


        public User User { get; set; }
       /* public Photo Photo { get; set; }
        public Competition Competition { get; set; }
        public Section Section { get; set; }*/
        public Participation Participation { get; set; }
    }
}
