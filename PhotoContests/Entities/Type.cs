using System.ComponentModel.DataAnnotations;

namespace PhotoContests.Entities
{
    public class Type
    {
        [Key]
        public int idType { get; set; }
        public string competitionType { get; set; }

        public ICollection<Competition> Competitions { get; set; }
    }
}
