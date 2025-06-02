using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;
using static System.Collections.Specialized.BitVector32;

namespace PhotoContests.Manager
{
    public class VotingManager:IVotingManager
    {
        private readonly IVotingRepo votingRepo;
        private readonly UserManager<User> userManager;
        private readonly IAssignementRepo assignementRepo;
        private readonly IParticipationRepo participationRepo;
        public VotingManager(IVotingRepo votingRepo, UserManager<User> userManager,
            IAssignementRepo assignementRepo, IParticipationRepo participationRepo)
        {
            this.votingRepo = votingRepo;
            this.userManager = userManager;
            this.assignementRepo= assignementRepo;
            this.participationRepo= participationRepo;
        }

        public void Create(VotingCreateModel model)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(model.emailUser)).Select(u => u.Id).FirstOrDefault();
            
            var user = users.Where(u => u.Email.Equals(model.emailUser)).FirstOrDefault();

            var participation = participationRepo.GetParticipationsSectionCompetitionPhoto()
               .Where(p => p.idPhoto == model.idPhoto && p.idSection == model.idSection && p.idCompetition == model.idCompetition)
               .FirstOrDefault();
            
            var votesPerPhoto = 0;
            var publicVote = 0;
            var jurorVote = 0;

            if (user is Juror juror)//verify the type of the user
            {
                //verify if the juror was assigned to this competition
                var correctAssignement = assignementRepo.GetAssignementsIQueryable()
                                            .Where(a => a.idCompetition == model.idCompetition && a.idUser == juror.Id)
                                            .Count();

                if (correctAssignement > 0)
                {
                    //verify if the juror didn't already vote for that photo for this competition and section 
                    var numberVotes = votingRepo.GetVotingIQueryable()
                        .Where(v => v.idUser == juror.Id && v.idPhoto == model.idPhoto && v.idCompetition == model.idCompetition && v.idSection == model.idSection)
                        .Count();
                    if (numberVotes == 0)
                        jurorVote = model.jurorVote;

                }
                else { return; }

            }
            else
            {
                var numberVotes = votingRepo.GetVotingIQueryable()
                        .Where(v => v.idUser == userId && v.idPhoto == model.idPhoto && v.idCompetition == model.idCompetition && v.idSection == model.idSection)
                        .Count();
                if (numberVotes == 0)
                { 
                    publicVote = 1;

                    votesPerPhoto = publicVote + votingRepo.GetVotingIQueryable()
                    .Where(v => v.idPhoto == model.idPhoto && v.idSection == model.idSection && v.idCompetition == model.idCompetition)
                    .Count();
                }
            }

            var newVoting = new Voting
            {
                idUser = userId,
                idPhoto = model.idPhoto,
                idCompetition = model.idCompetition,
                idSection = model.idSection,
                publicVote = publicVote,
                jurorVote = jurorVote
            };

            votingRepo.Create(newVoting);//now the vote was created
            
            //if the user was a photographer
            if (user is not Juror)
            {
                participation.totalPoints = votesPerPhoto;
                participationRepo.Update(participation);

                // now we have different totalPoints so we have to change the ranking
                RankingUpdate(model.idCompetition, model.idSection);
            }

        }

        public void RankingUpdate(int idCompetition, int idSection)
        {
            var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idCompetition && p.idSection == idSection)
                .OrderByDescending(p => p.totalPoints)
                .ToList();

            var currentValueRank = 1;

            participations[0].ranking = 1;
            currentValueRank++;
            for (int index=1; index < participations.Count;index++)
            {
                if (participations[index].totalPoints == participations[index-1].totalPoints)
                {
                    participations[index].ranking = participations[index - 1].ranking;
                }
                else
                {
                    participations[index].ranking = currentValueRank;
                    currentValueRank++;
                }
            }
            /*foreach(var participation in participations)
            {

                participation.ranking= currentValueRank;
                currentValueRank++;
            }*/

            participationRepo.UpdateRange(participations);
        }


        public bool VotedParticipation(VotedParticipationModel model)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(model.emailUser)).Select(u => u.Id).FirstOrDefault();

            var vote = votingRepo.GetVotingIQueryable()
                .Where(v => v.idPhoto== model.idPhoto && v.idSection == model.idSection && v.idCompetition == model.idCompetition && v.idUser == userId)
                .FirstOrDefault();
            if(vote == null)
            {
                return false;
            }
            return true;
        }

        public void Delete(string emailUser, int idPhoto, int idSection, int idCompetition)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(emailUser)).Select(u => u.Id).FirstOrDefault();
            var user = users.Where(u => u.Email.Equals(emailUser)).FirstOrDefault();

            var vote = votingRepo.GetVotingIQueryable()
                .FirstOrDefault(v => v.idPhoto == idPhoto && v.idSection == idSection && v.idCompetition == idCompetition && v.idUser == userId);

            if (vote == null) return;
            votingRepo.Delete(vote);

            var participation = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .FirstOrDefault(v => v.idPhoto == idPhoto && v.idSection == idSection && v.idCompetition == idCompetition);
            if (participation == null) return;
            
            if (user is not Juror)
            {
                participation.totalPoints--;
                participationRepo.Update(participation);

                // now we have different totalPoints so we have to change the ranking
                RankingUpdate(idCompetition, idSection);
            }

        }

        public int GetJurorVoteForParticipation(string emailJuror, int idPhoto, int idSection, int idCompetition)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(emailJuror)).Select(u => u.Id).FirstOrDefault();
            var user = users.Where(u => u.Email.Equals(emailJuror)).FirstOrDefault();
            if (user is not Juror) return -1;
            
            var vote = votingRepo.GetVotingIQueryable()
                .FirstOrDefault(v => v.idPhoto == idPhoto && v.idSection == idSection && v.idCompetition == idCompetition && v.idUser == userId);
            if (vote == null) return 0;
            return vote.jurorVote;
        }

        public void Update(VotingCreateModel model)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(model.emailUser)).Select(u => u.Id).FirstOrDefault();
            var user = users.Where(u => u.Email.Equals(model.emailUser)).FirstOrDefault();
            var vote = votingRepo.GetVotingIQueryable()
                .FirstOrDefault(v => v.idPhoto == model.idPhoto && v.idSection == model.idSection && v.idCompetition == model.idCompetition && v.idUser == userId);
            vote.jurorVote = model.jurorVote;
            votingRepo.Update(vote);
        }

    }
}
