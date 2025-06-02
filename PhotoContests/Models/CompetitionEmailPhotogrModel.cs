namespace PhotoContests.Models
{
    public class CompetitionEmailPhotogrModel
    {
        public string emailPh { get; set; }
        public string namePh { get; set; }
        public bool newsSubscription { get; set; }
        public List<PhotoDetailsReportedModel> photoDetails {get;set;}
    }
}
