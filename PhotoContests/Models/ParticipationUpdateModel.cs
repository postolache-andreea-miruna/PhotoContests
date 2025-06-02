namespace PhotoContests.Models
{
    public class ParticipationUpdateModel
    {
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }

        public int ranking { get; set; }
        public int totalPoints { get; set; } //total public points
        public double finalResult { get; set; }
    }
}
