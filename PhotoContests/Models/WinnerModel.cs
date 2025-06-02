namespace PhotoContests.Models
{
    public class WinnerModel
    {
        public int idPhoto { get; set; }
        public int totalPoints { get; set; }
        public double finalResult { get; set; }
        public int ranking { get; set; }
        public string idUser { get; set; }
        public string photoUrl { get; set; }
        public string photographerName { get; set; }
        public string profileUrl { get; set; }
        public string emailPhotographer { get; set; }

        public string title { get; set; }
        public DateTime? takenDate { get; set; }
    }
}
