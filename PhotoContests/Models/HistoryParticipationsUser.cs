using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PhotoContests.Models
{
    public class HistoryParticipationsUser
    {
        public string competitionName { get; set; }
        public int idCompetition { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string sectionName { get; set; }
        public int idPhoto { get; set; }
        public string photoUrl { get; set; }
        public int totalPoints { get; set; }
        public int ranking { get; set; }
        public double finalResult { get; set; }
    }
}
