namespace PhotoContests.Models
{
    public class BestOfModel
    {
        public string sectionName { get; set; }
        public int idCompetition { get; set; }
        public string competitionName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int idPhoto { get; set; }
        public string photoUrl { get; set; }
        public int ranking { get; set; }
      //  public int totalPoints { get; set; }
        public double finalResult { get; set; }
      //  public DateTime resultDate { get; set; }
    }
}
