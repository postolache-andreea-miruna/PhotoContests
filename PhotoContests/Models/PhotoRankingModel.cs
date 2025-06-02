using System.Globalization;

namespace PhotoContests.Models
{
    public class PhotoRankingModel
    {
        public int photoId { get; set; }
        public string photoUrl { get; set; }
        public int ranking { get; set; }
        public int totalPoints { get; set; }
    }
}
