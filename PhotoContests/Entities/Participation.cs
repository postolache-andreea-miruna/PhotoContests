namespace PhotoContests.Entities
{
    public class Participation
    {
        public int idPhoto { get; set; }
        public int idCompetition{ get; set; }
        public int idSection { get; set; }

        public int ranking { get;set; }
        public int totalPoints { get; set; } //total public points
        public double finalResult { get; set; }

        public string reportMessage { get; set; } = "";

        public Photo Photo { get; set; }
        public Competition Competition { get; set; }
        public Section Section { get; set; }

        public ICollection<Voting> Votings { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Video Video { get; set; }

        public ICollection<Report> Reports { get; set; }

    }
}
