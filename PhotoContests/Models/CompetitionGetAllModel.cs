using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Models
{
    public class CompetitionGetAllModel
    {
        public int idCompetition { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string urlPosterPhoto { get; set; }
        public bool status { get; set; }
        public string nameType { get; set; }
        public int photosLimit { get; set; }
    }
}
