namespace PhotoContests.Models
{
    public class AllCompetitionsActiveJurorModel
    {
        public int idCompetition { get; set; }
        public string name { get; set; }
        public DateTime endDate { get; set; }
        public string urlPosterPhoto { get; set; }
        public int noParticipants { get; set; }


        public DateTime startDate { get; set; }
        public string nameType { get; set; }
        public int photosLimit { get; set; }
        public int photoParticipants { get; set; }
        public DateTime resultDate { get; set; }
    }
}
