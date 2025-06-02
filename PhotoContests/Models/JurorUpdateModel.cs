namespace PhotoContests.Models
{
    public class JurorUpdateModel
    {
        public string emailJuror { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? profilePicture { get; set; }
        public string? biography { get; set; }
        public int yearsOfExperience { get; set; }
        public bool newsSubscription { get; set; }

    }
}
