using Microsoft.AspNetCore.Identity;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;

namespace PhotoContests.Manager
{
    public class UserMyManager:IUserMyManager
    {
        private readonly IUserRepo userRepo;
        private readonly UserManager<User> userManager;
        public UserMyManager(IUserRepo userRepo, UserManager<User> userManager)
        {
            this.userRepo = userRepo;
            this.userManager = userManager;
        }

        public PhotoEmail GetPhotoForEmail(string emailUser)
        {
            var users = userManager.Users;
            var idUser = users
                .Where(u => u.Email.Equals(emailUser))
                .Select(u => u.Id)
                .FirstOrDefault();
            var photo = userRepo.GetUsersIQueryable().Where(u => u.Id == idUser)
                .Select(u => new PhotoEmail
                {
                    photoUrl = u.profilePicture,
                    emailUser = u.Email
                })
                .FirstOrDefault();
            return photo;
        }

    }
}
