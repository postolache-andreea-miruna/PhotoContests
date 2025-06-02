namespace PhotoContests.Models
{
    public class UserAllActiveCompSectionModel
    {
        public int idSection { get; set; }
        public string sectionName { get; set; }
        public int rankingPerUser { get; set; }
        public int userTotalPoints { get; set; }
        public List<UserAllActiveCompSectionPhotosModel> photosDetails { get; set; }
    }
}
