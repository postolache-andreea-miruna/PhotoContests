namespace PhotoContests.Models
{
    public class ActiveCompetitionsUser
    {
        public string competition { get; set; }
        public DateTime endDate { get; set; }
        public string photoUrl { get; set; }
        public int ranking { get; set; }
        public int totalPoints { get; set; }
        public string sectionName { get; set; }
    }
}
