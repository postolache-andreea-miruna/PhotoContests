namespace PhotoContests.Models
{
    public class ParticipationCompSectPhotosPhModel
    {
        public int idPhoto { get; set; }
        public string photoUrl { get; set; }
        public int ranking { get; set; }
        public int totalPoints { get; set; }
        public string sectionName { get; set; }
        public double finalResult { get; set; }
    }
}
