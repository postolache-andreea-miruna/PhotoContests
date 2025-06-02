using System.ComponentModel.DataAnnotations;

namespace PhotoContests.Entities
{
    public class Section
    {
        [Key]
        public int idSection { get; set; }

        public string name { get; set; }
        public string detail { get; set; }
        public int minimumAge { get; set; } = 0;
        public string presentationUrl { get; set; }
        public string presentationYTCode { get; set; }

        public ICollection<Participation> Participations { get; set; }
    }
}
