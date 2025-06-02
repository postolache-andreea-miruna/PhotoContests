namespace PhotoContests.Models
{
    public class CompetitionSectionsModel
    {
        public string competitionName { get; set; }
        public DateTime endDate { get; set; }
        public List<SectionDetailModel> sections { get; set; }
    }
}
