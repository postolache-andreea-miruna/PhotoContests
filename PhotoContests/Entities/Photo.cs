using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Entities
{
    public class Photo
    {
        [Key]
        public int idPhoto { get; set; }
        public DateTime downloadTime { get; set; } = DateTime.Now;
        public string title { get; set; }
        public string description { get; set; }
        public string photoUrl { get; set; }
        public bool visibility { get; set; }

        [ForeignKey("Photographer")]
        public string idUser { get; set; }
        public Photographer Photographer { get; set; }

        public DateTime? takenDate { get; set; }
        public string? cameraModel { get; set; }
        public float? fStop { get; set; }
        public float? exposureTime { get; set; }
        public int? isoSpeed { get; set; }


        public ICollection<Participation> Participations { get; set; }
    }
}
