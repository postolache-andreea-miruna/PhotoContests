using Microsoft.AspNetCore.Identity;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;

namespace PhotoContests.Manager
{
    public class ReportManager :IReportManager
    {
        private readonly IReportRepo reportRepo;
        private readonly UserManager<User> userManager;
        private readonly IAssignementRepo assignementRepo;
        private readonly ICompetitionRepo competitionRepo;
        private readonly IParticipationRepo participationRepo;
        private readonly IVotingRepo votingRepo;

        public ReportManager(IReportRepo reportRepo, UserManager<User> userManager, IAssignementRepo assignementRepo,
            ICompetitionRepo competitionRepo, IParticipationRepo participationRepo,
            IVotingRepo votingRepo)
        {
            this.reportRepo = reportRepo;
            this.userManager = userManager;
            this.assignementRepo = assignementRepo;
            this.competitionRepo = competitionRepo;
            this.participationRepo = participationRepo;
            this.votingRepo = votingRepo;
        }

        public void Create(ReportCreateModel model)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(model.emailUser)).Select(u => u.Id).FirstOrDefault();
            var user = users.Where(u => u.Email.Equals(model.emailUser)).FirstOrDefault();

            if (user is Photographer photographer)
            {
                var newReport = new Report
                {
                    idUser = userId,
                    idCompetition = model.idCompetition,
                    idPhoto = model.idPhoto,
                    idSection = model.idSection,
                    message = model.message,
                    noAccept = 0,
                    noDecline = 0
                };
                reportRepo.Create(newReport);
            }
            else
            {//if it is a juror assigned to the competition

                var correctAssignement = assignementRepo.GetAssignementsIQueryable()
                    .Where(a => a.idCompetition == model.idCompetition && a.idUser == userId)
                    .Count();
                if(correctAssignement > 0)
                {
                    var newReportJuror = new Report
                    {
                        idUser = userId,
                        idCompetition = model.idCompetition,
                        idPhoto = model.idPhoto,
                        idSection = model.idSection,
                        message = model.message,
                        noAccept = model.noAccept,
                        noDecline = model.noDecline
                    };
                    reportRepo.Create(newReportJuror);
                }
                else
                {
                    return;
                }
            }
        }

        public NoDeclineAccModel GetAcceptDeclineReportJuror(int idCompetition, int idSection, int idPhoto, string emailJuror)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(emailJuror)).Select(u => u.Id).FirstOrDefault();

            var jurorsComp = assignementRepo.GetAssignementsIQueryable()
                .Where(a => a.idCompetition == idCompetition)
                .Select(a => a.idUser)
                .ToList();

            if (!jurorsComp.Contains(userId)) //if the user is not a juror for that competition
            {
                return new NoDeclineAccModel(); 
            }

            var reports = reportRepo.GetReportIQueryable()
                .Where(r => r.idCompetition == idCompetition && r.idSection == idSection && r.idUser == userId && r.idPhoto == idPhoto)
                .Select(r => new NoDeclineAccModel
                {
                    noAccept = r.noAccept,
                    noDecline = r.noDecline
                })
                .FirstOrDefault();
            return reports;

        }

        public ReportMessage GetReportMessage(int idCompetition, int idSection, int idPhoto,string emailUser)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(emailUser)).Select(u => u.Id).FirstOrDefault();

            var reportMessage = reportRepo.GetReportIQueryable()
                .Where(r => r.idCompetition == idCompetition && r.idSection==idSection&& r.idPhoto == idPhoto && r.idUser == userId)
                .Select(r => new ReportMessage{
                    message = r.message 
                })
                .FirstOrDefault();
            return reportMessage;
        }

        public bool GetExistParticipationReportsJuror(int idCompetition, int idSection, int idPhoto)//care nu au nici de la juror nici de la participanti
        {
            var numberReports = reportRepo.GetReportIQueryable()
               .Where(r => r.idCompetition == idCompetition && r.idSection == idSection && r.idPhoto == idPhoto)
               .Count();

            if(numberReports == 0) { return true; }
            return false;
        }
        public ReportsJurorModel GetAllParticipationPhReports(int idCompetition, int idSection, int idPhoto)
        {
            var jurorsComp = assignementRepo.GetAssignementsIQueryable()
                .Where(a => a.idCompetition == idCompetition)
                .Select(a => a.idUser)
                .ToList();

            var reports = reportRepo.GetReportIQueryable()
                .Where(r => r.idCompetition == idCompetition && r.idSection == idSection && r.idPhoto == idPhoto
                && !jurorsComp.Contains(r.idUser))
                .GroupBy(r => r.message)
                .Select(g => new AllReportNumber
                {
                    message = g.Key,
                    appearance = g.Count()
                })
               .ToList();

            if (reports.Count == 0)
            {
                return new ReportsJurorModel();
            }
            return new ReportsJurorModel
            {
                reportsSummary = reports
            };

        }
        public void ReportResult(int idCompetition) //cu o zi inainte de afisarea rezultatelor
        {
            var jurorsComp = assignementRepo.GetAssignementsIQueryable()
                .Where(a => a.idCompetition == idCompetition)
                .Select(a => a.idUser)
                .ToList();

            var timeNow = DateTime.Now;
            var competition = competitionRepo.GetCompetitionsIQueryable()
                .FirstOrDefault(c => c.idCompetition == idCompetition);
            /*if(competition != null && timeNow > competition.endDate
                 && timeNow < competition.resultsDate.AddHours(-1))*/
            if (competition != null
                 && timeNow >= competition.resultsDate.AddDays(-1)
                 && timeNow < competition.resultsDate)
            {
                var sectionsPhIds = participationRepo.GetParticipationsSectionCompetition()
                    .Where(p => p.idCompetition == idCompetition).Select(p => new { p.idSection, p.idPhoto })
                    .ToList();
                if(sectionsPhIds.Count > 0)
                {
                    for(int i =0;i<sectionsPhIds.Count;i++)
                    {
                        var report = reportRepo.GetReportIQueryable()
                             .Where(r => r.idPhoto == sectionsPhIds[i].idPhoto
                             && r.idCompetition == idCompetition
                             && r.idSection == sectionsPhIds[i].idSection
                             && jurorsComp.Contains(r.idUser))
                             .GroupBy(p => new { p.idCompetition, p.idPhoto, p.idSection })
                             .Select(g => new ReportResultModel
                             {
                                 noAcceptFinal = g.Count(r => r.noAccept == 1),
                                 noDeclineFinal = g.Count(r => r.noDecline == 1)
                             })
                             .FirstOrDefault();
                        if(report != null)
                        {
                            if(report.noDeclineFinal > report.noAcceptFinal)
                            {
                                var participation_ = participationRepo.GetParticipationsSectionCompetition()
                                    .Where(p => p.idPhoto == sectionsPhIds[i].idPhoto
                                    && p.idCompetition == idCompetition
                                    && p.idSection== sectionsPhIds[i].idSection)
                                    .FirstOrDefault();
                                participation_.reportMessage = "DENIED";
                                participation_.totalPoints= 0;
                                participation_.finalResult = 0;
                                participation_.ranking = 0;
                                participationRepo.Update(participation_);
                                
                                var participationVotes = votingRepo.GetVotingIQueryable()//all votes for that participation WILL be deleted
                                    .Where(v => v.idPhoto == sectionsPhIds[i].idPhoto
                                    && v.idCompetition == idCompetition
                                    && v.idSection == sectionsPhIds[i].idSection)
                                    .ToList();
                                votingRepo.DeleteVotes(participationVotes);

                                //update the ranking for all participations of this competition and section
                                
                                var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                                    .Where(p => p.idCompetition == idCompetition && p.idSection == sectionsPhIds[i].idSection && p.ranking!=0 && p.reportMessage!="DENIED")
                                    .OrderByDescending(p => p.totalPoints)
                                    .ToList();
                                /*
                                var currentValueRank = 1;
                                foreach (var participation in participations)
                                {
                                    participation.ranking = currentValueRank;
                                    currentValueRank++;
                                }

                                participationRepo.UpdateRange(participations);*/

                                var currentValueRank = 1;
                                participations[0].ranking = 1;
                                currentValueRank++;
                                for (int index = 1; index < participations.Count; index++)
                                {
                                    if (participations[index].totalPoints == participations[index - 1].totalPoints)
                                    {
                                        participations[index].ranking = participations[index - 1].ranking;
                                    }
                                    else
                                    {
                                        participations[index].ranking = currentValueRank;
                                        currentValueRank++;
                                    }
                                }
                                participationRepo.UpdateRange(participations);





                            }
                        }                    
                            
                    }
                }
            }
        }


    }
}
