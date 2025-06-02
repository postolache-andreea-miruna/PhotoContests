using Microsoft.AspNetCore.Identity;

namespace PhotoContests.Entities
{
    public class Role : IdentityRole
    {
        public ICollection<UserRoles> UsersRoles { get; set; }
    }
}
