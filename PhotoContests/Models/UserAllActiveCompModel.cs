namespace PhotoContests.Models
{
    public class UserAllActiveCompModel
    {    
        public int idCompetition { get; set; }
        public string name { get; set; }
        public DateTime endDate { get; set; }
        public DateTime startDate { get; set; }
        public string urlPosterPhoto { get; set; }
        public int noParticipants { get; set; }
        public int photosLimits { get; set; }
        public DateTime resultsDate { get; set; }

        public List<UserAllActiveCompSectionModel> sectionDetail { get; set; } 

    }
}
