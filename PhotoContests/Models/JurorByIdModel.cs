namespace PhotoContests.Models
{
    public class JurorByIdModel
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? profilePicture { get; set; }
        public string email { get; set; }
        public bool newsSubscription { get; set; }
    }
}
