namespace PhotoContests.Models
{
    public class HistoryParticipationsUserNewModel
    {
        public string competitionName { get; set; }
        public int idCompetition { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public List<SectionCompDetailModel> sectionsDetails { get; set; }
        
    }
}
