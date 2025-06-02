using System.ComponentModel.DataAnnotations;

namespace PhotoContests.Entities
{
    public class Nationality
    {
        [Key]
        public int idNationality { get; set; }
        public string name { get; set; }

        public ICollection<Photographer> Photographers { get; set; }

    }
}
