namespace PhotoContests.Models
{
    public class RegisterUserModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? profilePicture { get; set; }
        public string? biography { get; set; }

        public string email { get; set; }
        public string password { get; set; }
        public string idRole { get; set; }

        public bool newsSubscription { get; set; } //new

        //photographer
        public DateTime? dateOfBirth { get; set; }
        public int? idNationality { get; set; }

        //juror
        public int? yearsOfExperience { get; set; }
    }
}
