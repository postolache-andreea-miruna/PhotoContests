namespace PhotoContests.Models
{
    public class ReportCreateModel
    {
        public string emailUser { get; set; }
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }


        public string message { get; set; }
        public int noAccept { get; set; } 
        public int noDecline { get; set; } 
    }
}
