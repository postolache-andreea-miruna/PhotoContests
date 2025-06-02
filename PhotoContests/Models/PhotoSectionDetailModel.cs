namespace PhotoContests.Models
{
    public class PhotoSectionDetailModel
    {
        public int idPhoto { get; set; }
        public string photoUrl { get; set; }
        public int totalPoints { get; set; }
        public int ranking { get; set; }
        public double finalResult { get; set; }
    }
}
