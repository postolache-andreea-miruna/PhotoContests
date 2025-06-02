namespace PhotoContests.Models
{
    public class ParticipationModel
    {
        public string competition { get; set; }
        public int idCompetition { get; set; }
        public string competitionCover { get; set; }
        public DateTime endDateComp { get; set; }
        public string section { get; set; }
        public int ranking { get; set; }
        public int totalPoints { get; set; }
        public double finalResult { get; set; }

    }
}
