namespace PhotoContests.Models
{
    public class ReviewCreateModel
    {
        public string emailUser { get; set; }
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }
        public string message { get; set; }
        public DateTime sendDate { get; set; } 
    }
}
