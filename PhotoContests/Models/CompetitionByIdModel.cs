using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Models
{
    public class CompetitionByIdModel
    {
        public int idCompetition { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime resultsDate { get; set; }
        public string urlPosterPhoto { get; set; }
        public int photosLimit { get; set; }
        public bool status { get; set; }
        public string nameType { get; set; }
    }
}
