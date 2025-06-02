namespace PhotoContests.Models
{
    public class AllCompetitionFutureModel
    {
        public int idCompetition { get; set; }
        public string name { get; set; }
        public DateTime endDate { get; set; }
        public DateTime startDate { get; set; }
        public string urlPosterPhoto { get; set; }
        public int photosLimit { get; set; }
    }
}
