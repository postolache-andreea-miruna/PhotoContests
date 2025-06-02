namespace PhotoContests.Models
{
    public class VotedParticipationModel
    {
        public string emailUser { get; set; }
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }
    }
}
