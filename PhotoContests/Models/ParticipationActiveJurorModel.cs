namespace PhotoContests.Models
{
    public class ParticipationActiveJurorModel
    {
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }
        public string photoUrl { get; set; }
    }
}
