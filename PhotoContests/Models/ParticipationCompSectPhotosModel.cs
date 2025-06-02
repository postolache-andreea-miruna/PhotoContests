namespace PhotoContests.Models
{
    public class ParticipationCompSectPhotosModel
    {
        public int idPhoto { get; set; }
        public string photoUrl { get; set; }
        public string userName { get; set; }
        public int ranking { get;set; }
        public int totalPoints { get; set; }
        public double finalResult { get; set; }
        public string profileUrl { get; set; }
        public string emailPhotographer { get; set; }
    }
}
