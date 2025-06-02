using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Models
{
    public class PhotographerByEmailModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? profilePicture { get; set; }
        public string? biography { get; set; }
        public string email { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string nationality { get; set; }
        public bool newsSubscription { get; set; }
    }
}
