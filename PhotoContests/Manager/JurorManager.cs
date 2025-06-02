using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;
using static Azure.Core.HttpHeader;

namespace PhotoContests.Manager
{
    public class JurorManager:IJurorManager
    {
        private readonly IJurorRepo jurorRepo;
        private readonly IAssignementRepo assignementRepo;
        private readonly UserManager<User> userManager;
        public JurorManager (IJurorRepo jurorRepo, IAssignementRepo assignementRepo, UserManager<User> userManager)
        {
            this.jurorRepo = jurorRepo;
            this.assignementRepo = assignementRepo;
            this.userManager = userManager;
        }

        public JurorByEmailModel GetJurorInfo(string email)
        {
            var juror = jurorRepo.GetJurorsIQueryable().Where(j => j.Email == email)
                .Select(j => new JurorByEmailModel
                {
                    firstName = j.firstName,
                    lastName = j.lastName,
                    profilePicture= j.profilePicture,
                    biography= j.biography,
                    yearsOfExperience= j.yearsOfExperience,
                    email = j.Email,
                    newsSubscription = j.newsSubscription
                })
                .FirstOrDefault();
            return juror;
        }

        public JurorByEmailModel GetJurorInfoByID(string id)
        {
            var juror = jurorRepo.GetJurorsIQueryable().Where(j => j.Id == id)
                .Select(j => new JurorByEmailModel
                {
                    firstName = j.firstName,
                    lastName = j.lastName,
                    profilePicture = j.profilePicture,
                    biography = j.biography,
                    yearsOfExperience = j.yearsOfExperience,
                    email = j.Email,
                    newsSubscription = j.newsSubscription
                })
                .FirstOrDefault();
            return juror;
        }

        public JurorByIdModel GetJurorInfoById(string id)
        {
            var juror = jurorRepo.GetJurorsIQueryable().Where(j => j.Id == id)
                .Select(j => new JurorByIdModel
                {
                    id = id,
                    firstName = j.firstName,
                    lastName = j.lastName,
                    profilePicture = j.profilePicture,
                    email = j.Email,
                    newsSubscription = j.newsSubscription
                })
                .FirstOrDefault();
            return juror;
        }

        public List<JurorByIdModel> GetAllJurorsByCompetitionId(int competitionId)
        {

            /*var jurors = new List<JurorByIdModel>();
             var jurorsIds = assignementRepo.GetAssignementsIQueryable()
                 .Where(a => a.idCompetition == competitionId)
                 .Select(a => a.idUser)
                 .ToList();
             for (int i = 1; i <= jurorsIds.Count; i++)
             {
                 var juror = GetJurorInfoById(jurorsIds[i]);
                 jurors.Add(juror);
             }
             return jurors;*/
            if(assignementRepo == null)
            {
                return new List<JurorByIdModel>();
            }
            var jurors = assignementRepo.GetAssignementsIQueryable()
                .Include(a => a.Juror)
                .Where(a => a.idCompetition == competitionId)
                .Select(a => new JurorByIdModel
                {
                    id = a.idUser,
                    firstName = a.Juror.firstName,
                    lastName = a.Juror.lastName,
                    profilePicture = a.Juror.profilePicture,
                    email = a.Juror.Email,
                    newsSubscription = a.Juror.newsSubscription
                })
                .OrderBy(a => a.lastName)
                .ToList();
           
            return jurors;
        }

        public List<JurorByIdModel> GetAllJurorsNotInCompById(int competitionId)
        {

            if (assignementRepo.GetAssignementsIQueryable().Count() == 0)
            {
                var jurors_ = jurorRepo.GetJurorsIQueryable()
                .Select(a => new JurorByIdModel
                {
                    id = a.Id,
                    firstName = a.firstName,
                    lastName = a.lastName,
                    profilePicture = a.profilePicture,
                    email = a.Email,
                    newsSubscription = a.newsSubscription
                })
                .OrderBy(a => a.lastName)
                .ToList();
                return jurors_;
            }
            var result = new List<JurorByIdModel>();

            var all_jurors = jurorRepo.GetJurorsIQueryable()
                .Select(a => new JurorByIdModel
                {
                    id = a.Id,
                    firstName = a.firstName,
                    lastName = a.lastName,
                    profilePicture = a.profilePicture,
                    email = a.Email,
                    newsSubscription = a.newsSubscription
                })
                .OrderBy(a => a.lastName)
            .ToList();

            var jurors = assignementRepo.GetAssignementsIQueryable()
                .Include(a => a.Juror)
                .Where(a => a.idCompetition == competitionId)
                .Select(a => new JurorByIdModel
                {
                    id = a.idUser,
                    firstName = a.Juror.firstName,
                    lastName = a.Juror.lastName,
                    profilePicture = a.Juror.profilePicture,
                    email = a.Juror.Email,
                    newsSubscription = a.Juror.newsSubscription
                })
                .OrderBy(a => a.lastName)
            .ToList();

            if(jurors.Count == 0)
            {
                var jurors_ = jurorRepo.GetJurorsIQueryable()
                .Select(a => new JurorByIdModel
                {
                    id = a.Id,
                    firstName = a.firstName,
                    lastName = a.lastName,
                    profilePicture = a.profilePicture,
                    email = a.Email,
                    newsSubscription = a.newsSubscription
                })
                .OrderBy(a => a.lastName)
                .ToList();
                return jurors_;
            }

            List<JurorByIdModel> diff = all_jurors.Where(j => !jurors.Any(jr => jr.id == j.id)).ToList();


/*            for (int juror_comp_index = 0; juror_comp_index < jurors.Count; juror_comp_index++)
            {
                for(int jurors_all_index = 0; jurors_all_index<all_jurors.Count; jurors_all_index++)
                {
                    if (jurors[juror_comp_index].id != all_jurors[jurors_all_index].Id)
                    {
                        result.Add(new JurorByIdModel
                        {
                            id = all_jurors[jurors_all_index].Id,
                            firstName = all_jurors[jurors_all_index].firstName,
                            lastName = all_jurors[jurors_all_index].lastName,
                            profilePicture = all_jurors[jurors_all_index].profilePicture,
                            email = all_jurors[jurors_all_index].Email
                        });
                    }
                }
            }*/

            return diff;
        }

        public List<JurorGetAllModel> GetAllJurors()
        {
            var juror = jurorRepo.GetJurorsIQueryable()
                .Select(j => new JurorGetAllModel
                {
                    firstName = j.firstName,
                    lastName = j.lastName,
                    profilePicture = j.profilePicture,
                    yearsOfExperience = j.yearsOfExperience,
                    email = j.Email
                })
                .OrderByDescending(j => j.yearsOfExperience)
                .ToList();
            return juror;
        }

        public void Update(JurorUpdateModel model)
        {
            var users = userManager.Users;
            var jurorUser = users
                .Where(u => u.Email.Equals(model.emailJuror))
                .FirstOrDefault();
            var juror = jurorRepo.GetJurorsIQueryable().FirstOrDefault(p => p.Email == model.emailJuror);

            if (jurorUser == null || juror == null) return;

            juror.profilePicture = model.profilePicture;
            juror.firstName = model.firstName;
            juror.lastName = model.lastName;
            juror.biography = model.biography;
            juror.yearsOfExperience= model.yearsOfExperience;
            juror.newsSubscription = model.newsSubscription;

            jurorRepo.Update(juror);
        }
    }
}
