namespace PhotoContests.Models
{
    public class PortofolioPhotosOtherViewModel
    {
        public int idPhoto { get; set; }
        public string photoUrl { get; set; }
        public DateTime downloadTime { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public List<ParticipationModel> results { get; set; }
    }
}
