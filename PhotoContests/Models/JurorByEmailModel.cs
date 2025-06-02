namespace PhotoContests.Models
{
    public class JurorByEmailModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? profilePicture { get; set; }
        public string? biography { get; set; }
        public int yearsOfExperience { get; set; }
        public string email { get; set; }
        public bool newsSubscription { get; set; }
    }
}
