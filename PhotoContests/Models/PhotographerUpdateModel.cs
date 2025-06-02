namespace PhotoContests.Models
{
    public class PhotographerUpdateModel
    {
        public string emailPhotographer { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? profilePicture { get; set; }
        public string? biography { get; set; }
        public string nationality { get; set; }
        public bool newsSubscription { get; set; }
    }
}
