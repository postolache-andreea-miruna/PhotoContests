namespace PhotoContests.Models
{
    public class VotingCreateModel
    {
        public string emailUser { get; set; }
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }
        public int jurorVote { get; set; }
    }
}
