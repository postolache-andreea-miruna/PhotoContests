using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Models
{
    public class GetPhotoModel
    {
        public int idPhoto { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string photoUrl { get; set; }
        public bool visibility { get; set; }
        public string namePhotographer { get; set; }
        public DateTime downloadTime { get; set; }

        public DateTime? takenDate { get; set; }
        public string? cameraModel { get; set; }
        public float? fStop { get; set; }
        public float? exposureTime { get; set; }
        public int? isoSpeed { get; set; }

        public string emailPhotographer { get; set; }
        public bool newsSubscription { get; set; }
    }
}
