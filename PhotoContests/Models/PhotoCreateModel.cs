using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoContests.Models
{
    public class PhotoCreateModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string photoUrl { get; set; }
        public bool visibility { get; set; }
        public string emailUser { get; set; }
        public DateTime? takenDate { get; set; }
        public string? cameraModel { get; set; }
        public float? fStop { get; set; }
        public float? exposureTime { get; set; }
        public int? isoSpeed { get; set; }

    }
}
