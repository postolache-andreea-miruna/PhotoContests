using Microsoft.AspNetCore.Identity;

namespace PhotoContests.Entities
{
    public class User : IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string?  profilePicture { get; set; }
        public string? biography { get; set; }

        public bool availability { get; set; }
        public bool newsSubscription { get; set; } = true;

        public string? connectionCode { get; set; } = null;

        public ICollection<UserRoles> UsersRoles { get; set; }
        public ICollection<Voting> Votings { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Message> Messages { get; set; }

        public ICollection<Report> Reports { get; set; }
    }
}
