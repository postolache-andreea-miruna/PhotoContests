using Microsoft.AspNetCore.Identity;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;

namespace PhotoContests.Manager
{
    public class PhotographerManager:IPhotographerManager
    {
        private readonly IPhotographerRepo photographerRepo;
        private readonly UserManager<User> userManager;
        private readonly INationalityRepo nationalityRepo;
        public PhotographerManager(IPhotographerRepo photographerRepo, UserManager<User> userManager, INationalityRepo nationalityRepo)
        {
            this.photographerRepo = photographerRepo;
            this.userManager = userManager;
            this.nationalityRepo= nationalityRepo;
        }

        public PhotographerByEmailModel GetPhotographerInfo(string email)
        {
            var photographer = photographerRepo.GetPhotographersIQueryable().Where(p => p.Email == email)
                .Select(p => new PhotographerByEmailModel
                {
                    firstName = p.firstName,
                    lastName = p.lastName,
                    profilePicture= p.profilePicture,
                    biography= p.biography,
                    email = p.Email,
                    dateOfBirth = p.dateOfBirth,
                    nationality = p.Nationality.name,
                    newsSubscription = p.newsSubscription
                })
                .FirstOrDefault();
            return photographer;
        }

        public PhotographerByEmailModel GetPhotographerInfoById(string id)
        {
            var photographer = photographerRepo.GetPhotographersIQueryable().Where(p => p.Id == id)
                .Select(p => new PhotographerByEmailModel
                {
                    firstName = p.firstName,
                    lastName = p.lastName,
                    profilePicture = p.profilePicture,
                    biography = p.biography,
                    email = p.Email,
                    dateOfBirth = p.dateOfBirth,
                    nationality = p.Nationality.name,
                    newsSubscription = p.newsSubscription
                })
                .FirstOrDefault();
            return photographer;
        }

        public void Update(PhotographerUpdateModel model)
        {
            var users = userManager.Users;
            var photographerUser = users
                .Where(u => u.Email.Equals(model.emailPhotographer))
                .FirstOrDefault();
            var photographer = photographerRepo.GetPhotographersIQueryable().FirstOrDefault(p => p.Email == model.emailPhotographer);
            var idNationality = nationalityRepo.GetNationalitiesIQueryable().Where(n => n.name == model.nationality)
                .Select(n => n.idNationality).FirstOrDefault();
            if (photographerUser == null || photographer == null) return;

            photographer.profilePicture = model.profilePicture;
            photographer.firstName = model.firstName;
            photographer.lastName = model.lastName;
            photographer.biography= model.biography;
            photographer.newsSubscription = model.newsSubscription;
            photographer.idNationality = idNationality;

            photographerRepo.Update(photographer);
        }
    }
}
