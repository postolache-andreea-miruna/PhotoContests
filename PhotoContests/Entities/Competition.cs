using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Entities
{
    public class Competition
    {
        [Key]
        public int idCompetition { get; set; }

        public string name { get; set; }
        public string description { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime resultsDate { get; set; }
        public string urlPosterPhoto { get; set; }
        public int photosLimit { get; set; }
        public bool status { get; set; }

        [ForeignKey("Type")]
        public int idType { get; set; }
        public Type Type { get; set; }
        public DateTime? datePhotoTakenLimit { get; set; }

        public ICollection<Participation> Participations { get; set; }
        public ICollection<Assignement> Assignements { get; set; }
    }
}
