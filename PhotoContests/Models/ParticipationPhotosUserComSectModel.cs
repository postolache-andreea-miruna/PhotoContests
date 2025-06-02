namespace PhotoContests.Models
{
    public class ParticipationPhotosUserComSectModel
    {
        public string userName { get; set; }
        public int ranking { get; set; }
        public string profilePicture { get; set; }
        public int totalPoints { get; set; }
        public string emailPhotographer { get; set; }
        public List<ParticipationPhotoModel> photos { get; set; } = new List<ParticipationPhotoModel>();
    }
}
