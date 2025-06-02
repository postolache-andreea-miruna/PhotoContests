using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;
using System.Runtime.CompilerServices;

namespace PhotoContests.Manager
{
    public class AssignementManager:IAssignementManager
    {
        private readonly IAssignementRepo assignementRepo;
        private readonly IJurorManager jurorManager;
        private readonly UserManager<User> userManager;
        public AssignementManager(IAssignementRepo assignementRepo, IJurorManager jurorManager,
            UserManager<User> userManager)
        {
            this.assignementRepo = assignementRepo;
            this.jurorManager = jurorManager;
            this.userManager = userManager;
        }

        public void Create(AssignementCreateModel model)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(model.emailUser)).Select(u => u.Id).FirstOrDefault();

            var newAssignement = new Assignement
            {
                idUser = userId,
                idCompetition = model.idCompetition
            };

            assignementRepo.Create(newAssignement);

        }

        public void Delete(string emailJuror, int idCompetition)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(emailJuror)).Select(u => u.Id).FirstOrDefault();

            var assignement = assignementRepo.GetAssignementsIQueryable()
                .FirstOrDefault(a => a.idUser == userId && a.idCompetition == idCompetition);
            if (assignement == null) return;
            assignementRepo.Delete(assignement);
        }

        /* public List<JurorByIdModel> GetAllJurorsByCompetitionId(int competitionId)
         {
             var jurors = new List<JurorByIdModel>();
             var jurorsIds = assignementRepo.GetAssignementsIQueryable()
                 .Where(a => a.idCompetition == competitionId)
                 .Select(a => a.idUser)
                 .ToList();
             for(int i = 1; i <= jurorsIds.Count; i++)
             {
                 var juror = jurorManager.GetJurorInfoById(jurorsIds[i]);
                 jurors.Add(juror);
             }
             return jurors;
         }*/
    }
}
