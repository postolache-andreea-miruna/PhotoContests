namespace PhotoContests.Models
{
    public class CompetitionGetAllParticpModel
    {
        public int idCompetition { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string urlPosterPhoto { get; set; }
        public bool status { get; set; }
        public string nameType { get; set; }
        public int photosLimit { get; set; }
        public int photoParticipants { get; set; }
    }
}
