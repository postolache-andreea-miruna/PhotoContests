using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Entities
{
    public class Photographer: User
    {
        public DateTime dateOfBirth { get; set; }


        [ForeignKey("Nationality")]
        public int idNationality { get; set; }
        public Nationality Nationality { get; set; }
        public ICollection<Photo> Photos { get; set; }
      //  public ICollection<Video> Videos { get; set; }
    }
}
