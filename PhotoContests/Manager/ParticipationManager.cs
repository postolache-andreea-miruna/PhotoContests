using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using PhotoContests.Entities;
using PhotoContests.Migrations;
using PhotoContests.Models;
using PhotoContests.Repo;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

namespace PhotoContests.Manager
{
    public class ParticipationManager:IParticipationManager
    {
        private readonly IParticipationRepo participationRepo;
        private readonly ICompetitionRepo competitionRepo;
        private readonly UserManager<User> userManager;
        private readonly IPhotoRepo photoRepo;
        private readonly ISectionRepo sectionRepo;
        private readonly IVotingRepo votingRepo;
        private readonly IPhotographerRepo photographerRepo;
        private readonly IReportRepo reportRepo;
        private readonly IAssignementRepo assignementRepo;
        public ParticipationManager(IParticipationRepo participationRepo,
            UserManager<User> userManager,
            ICompetitionRepo competitionRepo,
            IPhotoRepo photoRepo,
            ISectionRepo sectionRepo,
            IPhotographerRepo photographerRepo,
            IVotingRepo votingRepo,
            IReportRepo reportRepo,
            IAssignementRepo assignementRepo
            )
        {
            this.participationRepo = participationRepo;
            this.userManager = userManager;
            this.competitionRepo = competitionRepo;
            this.photoRepo = photoRepo;
            this.sectionRepo = sectionRepo;
            this.photographerRepo = photographerRepo;
            this.votingRepo = votingRepo;
            this.reportRepo = reportRepo;
            this.assignementRepo = assignementRepo;
        }

        public int Create(ParticipationCreateModel model)
        {
            var ageProblem = false;//1
            var photoTakenDateProblem = false;//2
            var numberPhotoSubmittedPb = false;//3
            var message = "";
            var photo = photoRepo.GetPhotosIQueryable().Where(p => p.idPhoto == model.idPhoto).FirstOrDefault();
            var section = sectionRepo.GetSectionsIQueryable().Where(s => s.idSection== model.idSection).FirstOrDefault();  
            var competition = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == model.idCompetition).FirstOrDefault();

            var photographer = photographerRepo.GetPhotographersIQueryable().Where(p => p.Id == photo.idUser).FirstOrDefault();


            var photographerBirthDate = photographer.dateOfBirth;
            var currentDate = DateTime.Now;
            var photograherAge = currentDate.Year - photographerBirthDate.Year;
            if(currentDate < photographerBirthDate.AddYears(photograherAge)) { photograherAge--; }

            var countParticipations = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => p.idCompetition == model.idCompetition && p.idSection == model.idSection)
                .Count();

            var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
               .Where(p => p.idCompetition == model.idCompetition && p.idSection == model.idSection)
               .OrderByDescending(p => p.totalPoints)
               .ToList();
            var lastParticipation = participations.LastOrDefault();

            var countUsedParticipationComp = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == model.idCompetition && p.Photo.idUser == photo.idUser)
                .Count();

            if(photograherAge < section.minimumAge) { ageProblem = true; message = "You are not older enough to participate";
                return 1;
            }
            if (photo.takenDate < competition.datePhotoTakenLimit && competition.datePhotoTakenLimit != null) {
                photoTakenDateProblem= true; message = "The photo doesn't respect the date constraint";
                return 2;
            }
            if (countUsedParticipationComp == competition.photosLimit) 
            { numberPhotoSubmittedPb = true; 
                message = "You submitted the limit number of photos for this competition";
                return 3; }

            if (photograherAge >= section.minimumAge && countUsedParticipationComp < competition.photosLimit && ((photo.takenDate>=competition.datePhotoTakenLimit && competition.datePhotoTakenLimit!=null) || competition.datePhotoTakenLimit==null))//////////modif pentru a considera pozele realizate dupa limita
            {
                var rankNew = 1;
                if (lastParticipation != null)
                {
                    if (lastParticipation.totalPoints == 0)
                    {
                        rankNew = lastParticipation.ranking;
                    }
                    else
                    {
                        rankNew = lastParticipation.ranking + 1;
                    }
                }                
                
                var newParticipation = new Participation
                {
                    idCompetition = model.idCompetition,
                    idPhoto = model.idPhoto,
                    idSection = model.idSection,
                    totalPoints = 0,
                    finalResult = 0,
                    ranking = rankNew //countParticipations + 1
                };
                participationRepo.Create(newParticipation);
                return 0;
            }
            return 4;

        }

        public void Delete(int idPhoto, int idSection, int idCompetition)
        {
            var participation = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .FirstOrDefault(p => p.idPhoto == idPhoto && p.idSection == idSection && p.idCompetition == idCompetition);
            if (participation == null) return;
            //verify if the participation has public votes
            var publicVotes = votingRepo.GetVotingIQueryable()
                .Where(v => v.idPhoto == idPhoto && v.idSection == idSection && v.idCompetition == idCompetition)
                .ToList();
            if(publicVotes.Count > 0)
            {
                votingRepo.DeleteVotes(publicVotes);
            }
            //verify if the participation has reports
            var reports = reportRepo.GetReportIQueryable()
                .Where(r => r.idPhoto == idPhoto && r.idSection == idSection && r.idCompetition == idCompetition)
                .ToList();
            if(reports.Count > 0)
            {
                reportRepo.DeleteReports(reports);
            }
            
            participationRepo.Delete(participation);
        }

        //nou
        public void UpdateNew(Participation2UpdateModel model)
        {
            var competition = competitionRepo.GetCompetitionsIQueryable()
                                  .FirstOrDefault(c => c.idCompetition == model.idCompetition);

            if (competition == null) return;

            //aici e cand aveam si idSection
           /* var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == model.idCompetition && p.idSection == model.idSection && p.ranking != 0)
                .ToList();

            if (participations == null || !participations.Any()) return;*/

            var timeNow = DateTime.Now;

            // Verifică dacă este cu o oră înainte de rezultatele competiției
            if (competition.resultsDate - timeNow <= TimeSpan.FromHours(24))//12
            {
                var sections = participationRepo.GetParticipationsSectionCompetitionPhoto()
                                .Where(p => p.idCompetition == model.idCompetition)
                                .Select(p => p.idSection)
                                .Distinct()
                                .ToList();
                foreach (var sectionId in sections)
                {
                    var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                        .Where(p => p.idCompetition == model.idCompetition && p.idSection == sectionId && p.ranking != 0)
                        .ToList();
                    if (!participations.Any()) continue;


                    var totalPublicPhotosSectionVotes = votingRepo.GetVotingIQueryable()
                    .Where(v => v.idSection == sectionId && v.idCompetition == model.idCompetition && v.jurorVote == 0)
                    .Count();

                    foreach (var participation in participations)
                    {
                        var publicPercPhotoVotes = (participation.totalPoints / (double)totalPublicPhotosSectionVotes) * 100;

                        var jurorVotes = votingRepo.GetVotingIQueryable()
                            .Where(v => v.idPhoto == participation.idPhoto && v.idSection == sectionId && v.idCompetition == model.idCompetition && v.publicVote == 0)
                            .Select(v => v.jurorVote)
                            .ToList();

                        var avgJurorVotes = jurorVotes.Any() ? jurorVotes.Average() * 10 : 0;
                        participation.finalResult = (publicPercPhotoVotes * 0.3) + (avgJurorVotes * 0.7);
                    }

                    participationRepo.UpdateRange(participations);
                    RankingUpdateFinal(model.idCompetition, sectionId);
                }
            }
        }



        public void Update(ParticipationUpdateModel model) //nu il mai folosesc
        {
            var competition = competitionRepo.GetCompetitionsIQueryable()
                              .Where(c => c.idCompetition == model.idCompetition)
                              .FirstOrDefault();

            if (competition == null) return;

            var participation = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .FirstOrDefault(p => p.idCompetition == model.idCompetition && p.idSection == model.idSection && p.idPhoto == model.idPhoto && p.ranking!=0);

            if(participation == null) return;   

            var timeNow = DateTime.Now;
            
            if (timeNow >= competition.resultsDate)
            {
                var totalPublicPhotosSectionVotes = votingRepo.GetVotingIQueryable()
                    .Where(v => v.idSection == model.idSection && v.idCompetition == model.idCompetition && v.jurorVote == 0)
                    .Count();
                var publicPercPhotoVotes = (participation.totalPoints / totalPublicPhotosSectionVotes) * 100;
            
                var jurorVotes = votingRepo.GetVotingIQueryable()
                        .Where(v => v.idPhoto == model.idPhoto && v.idSection == model.idSection && v.idCompetition == model.idCompetition && v.publicVote == 0)
                        .Select(v => v.jurorVote)
                        .ToList();
               var avgJurorVotes = jurorVotes.Average() * 10; //to have the percentage (the average was divided by 10 (the maxim number) and then multiply by 100)
               participation.finalResult = (publicPercPhotoVotes * 0.3) + (avgJurorVotes * 0.7);
               participationRepo.Update(participation);
               RankingUpdateFinal(model.idCompetition, model.idSection);
            }
        }
        public void RankingUpdateFinal(int idCompetition, int idSection)
        {
            /*var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idCompetition && p.idSection == idSection && p.ranking!=0)
                .OrderByDescending(p => p.finalResult)
                .ToList();

            var currentValueRank = 1;
            foreach (var participation in participations)
            {
                participation.ranking = currentValueRank;
                currentValueRank++;
            }
            participationRepo.UpdateRange(participations);*/
            //varianta noua
            var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idCompetition && p.idSection == idSection && p.ranking != 0)
                .OrderByDescending(p => p.finalResult)
                .ToList();

            var currentValueRank = 1;
            participations[0].ranking = 1;
            currentValueRank++;

            for(int index = 1; index < participations.Count; index++)
            {
                if(participations[index].finalResult == participations[index-1].finalResult)
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

        public List<ParticipationCompSectPhotosModel> GetAllParticipationPhotosByCompSect(int idComp, int idSect)
        {
            var timeNow = DateTime.Now;
            var competition = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == idComp).FirstOrDefault();
            if (timeNow < competition.resultsDate)
            {
                var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idComp && p.idSection == idSect)
                .Select(p => new ParticipationCompSectPhotosModel
                {
                    idPhoto = p.idPhoto,
                    photoUrl = p.Photo.photoUrl,
                    userName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                    ranking = p.ranking,
                    totalPoints = p.totalPoints,
                    finalResult = p.finalResult,
                    profileUrl = p.Photo.Photographer.profilePicture,
                    emailPhotographer = p.Photo.Photographer.Email
                })
                .OrderByDescending(p => p.totalPoints)
                .ToList();
                return participations;
            }
            else 
            {
                var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idComp && p.idSection == idSect && p.ranking!=0)
                .Select(p => new ParticipationCompSectPhotosModel
                {
                    idPhoto = p.idPhoto,
                    photoUrl = p.Photo.photoUrl,
                    userName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                    ranking = p.ranking,
                    totalPoints = p.totalPoints,
                    finalResult = p.finalResult,
                    profileUrl = p.Photo.Photographer.profilePicture,
                    emailPhotographer = p.Photo.Photographer.Email
                })
                .OrderByDescending(p => p.finalResult)
                .ToList();
                return participations;
            }
        }

        public List<ParticipationCompSectPhotosModel> GetAllUserParticipationPhotosByCompSect(int idComp, int idSect, string email)
        {
            var timeNow = DateTime.Now;
            var competition = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == idComp).FirstOrDefault();
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (timeNow < competition.resultsDate)
            {
                var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idComp && p.idSection == idSect && p.Photo.Photographer.Id == idPhotographer)
                .Select(p => new ParticipationCompSectPhotosModel
                {
                    idPhoto = p.idPhoto,
                    photoUrl = p.Photo.photoUrl,
                    userName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                    ranking = p.ranking,
                    totalPoints = p.totalPoints,
                    finalResult = p.finalResult,
                    profileUrl = p.Photo.Photographer.profilePicture,
                    emailPhotographer = p.Photo.Photographer.Email
                })
                .OrderByDescending(p => p.totalPoints)
                .ToList();
                return participations;
            }
            else
            {
                var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idComp && p.idSection == idSect && p.Photo.Photographer.Id == idPhotographer && p.ranking!=0)
                .Select(p => new ParticipationCompSectPhotosModel
                {
                    idPhoto = p.idPhoto,
                    photoUrl = p.Photo.photoUrl,
                    userName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                    ranking = p.ranking,
                    totalPoints = p.totalPoints,
                    finalResult = p.finalResult,
                    profileUrl = p.Photo.Photographer.profilePicture,
                    emailPhotographer = p.Photo.Photographer.Email
                })
                .OrderBy(p => p.ranking)
                .ToList();
                return participations;
            }
        }

        //afiseaza participarile la sectiunea curenta per utilizator in ordinea descrescatoare a punctelor
        public List<ParticipationPhotosUserComSectModel> GetAllParticipationPhotosByCompSectGrouped(int idComp, int idSect)
        {
            var participants = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idComp && p.idSection == idSect)
                .GroupBy(p => new
                {
                    p.Photo.Photographer.firstName,
                    p.Photo.Photographer.lastName,
                    p.Photo.Photographer.Id,
                    p.Photo.Photographer.profilePicture,
                    p.Photo.Photographer.Email,
                })
                .Select(g => new 
                {
                    userId = g.Key.Id,
                    userName = g.Key.firstName + " " + g.Key.lastName,
                    profilePicture = g.Key.profilePicture,
                    totalPoints = g.Sum(p => p.totalPoints),
                    emailPhotographer = g.Key.Email,
                    photos = g.Select(p => new ParticipationPhotoModel
                    {
                        idPhoto = p.idPhoto,
                        photoUrl = p.Photo.photoUrl,
                        points = p.totalPoints
                    }).ToList()
                })
                .OrderByDescending(p => p.totalPoints)
                .ToList();

            var rankedParticipants = participants.Select((user,counter) => new ParticipationPhotosUserComSectModel
            {
                userName = user.userName,
                totalPoints = user.totalPoints,
                profilePicture = user.profilePicture,
                emailPhotographer = user.emailPhotographer,
                ranking = counter + 1,
                photos = user.photos
            }).ToList();

            return rankedParticipants;
        }


        public ParticipationPhotosUserComSectModel GetAllMyParticipationPhotosByCompSectUser(int idComp, int idSect, string email)
        {
            var myParticipation = new ParticipationPhotosUserComSectModel();
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();

            var participants = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idComp && p.idSection == idSect)
                .GroupBy(p => new
                {
                    p.Photo.Photographer.firstName,
                    p.Photo.Photographer.lastName,
                    p.Photo.Photographer.Id,
                    p.Photo.Photographer.profilePicture,
                    p.Photo.Photographer.Email,
                })
                .Select(g => new
                {
                    userId = g.Key.Id,
                    userName = g.Key.firstName + " " + g.Key.lastName,
                    profilePicture = g.Key.profilePicture,
                    totalPoints = g.Sum(p => p.totalPoints),
                    emailPhotographer = g.Key.Email,
                    photos = g.Select(p => new ParticipationPhotoModel
                    {
                        idPhoto = p.idPhoto,
                        photoUrl = p.Photo.photoUrl,
                        points = p.totalPoints
                    }).ToList()
                })
                .OrderByDescending(p => p.totalPoints)
                .ToList();

            var rankedParticipants = participants.Select((user, counter) => new 
            {
                idUser = user.userId,
                userName = user.userName,
                totalPoints = user.totalPoints,
                emailPhotographer = user.emailPhotographer,
                profilePicture = user.profilePicture,
                ranking = counter + 1,
                photos = user.photos
            }).ToList();

            for(int index=0; index<rankedParticipants.Count;index++)
            {
                if (rankedParticipants[index].idUser == idPhotographer)
                {
                    myParticipation.userName = rankedParticipants[index].userName;
                    myParticipation.totalPoints = rankedParticipants[index].totalPoints;
                    myParticipation.profilePicture = rankedParticipants[index].profilePicture;
                    myParticipation.emailPhotographer = rankedParticipants[index].emailPhotographer;
                    myParticipation.ranking = rankedParticipants[index].ranking;
                    myParticipation.photos = rankedParticipants[index].photos;
                }
            }
            return myParticipation;
        }

        public List<ParticipationCompSectPhotosPhModel> GetAllParticipationForUserComp(string email, int idComp)
        {
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (idPhotographer == null) { return new List<ParticipationCompSectPhotosPhModel>(); }
            var timeNow = DateTime.Now;
            var competition = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == idComp).FirstOrDefault();
            if (timeNow < competition.resultsDate)
            { var participationscomp = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idComp && p.Photo.idUser == idPhotographer)
                .Select(p => new ParticipationCompSectPhotosPhModel
                {
                    idPhoto = p.idPhoto,
                    photoUrl = p.Photo.photoUrl,
                    ranking = p.ranking,
                    totalPoints = p.totalPoints,
                    sectionName = p.Section.name,
                    finalResult = p.finalResult
                })
                .OrderBy(p => p.sectionName).ThenByDescending(p => p.totalPoints)
                .ToList();
                return participationscomp;
            }
            else
            {
                var participationscomp = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idComp && p.Photo.idUser == idPhotographer && p.ranking!=0)
                .Select(p => new ParticipationCompSectPhotosPhModel
                {
                    idPhoto = p.idPhoto,
                    photoUrl = p.Photo.photoUrl,
                    ranking = p.ranking,
                    totalPoints = p.totalPoints,
                    sectionName = p.Section.name,
                    finalResult = p.finalResult
                })
                .OrderBy(p => p.sectionName).ThenByDescending(p => p.finalResult)
                .ToList();

                return participationscomp;
            }
        }

        public List<HistoryParticipationsUser> GetParticipationHistoryUser(string email)
        {
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (idPhotographer == null) { return new List<HistoryParticipationsUser>(); }

            var history = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.Photo.idUser == idPhotographer)
                .Select(p => new HistoryParticipationsUser
                {
                    competitionName = p.Competition.name,
                    idCompetition = p.Competition.idCompetition,
                    startDate = p.Competition.startDate,
                    endDate = p.Competition.endDate,

                    sectionName = p.Section.name,
                    idPhoto = p.idPhoto,
                    photoUrl = p.Photo.photoUrl,

                    totalPoints= p.totalPoints,
                    ranking = p.ranking,
                    finalResult = p.finalResult
                })
                .OrderByDescending(p => p.startDate) 
                .ThenBy(p => p.competitionName)
                .ThenBy(p => p.sectionName)
                .ToList();
            return history;
        }

        public List<HistoryParticipationsUserNewModel> GetParticipationHistoryUserGrouped(string email)
        {
            var timeNow = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (idPhotographer == null) { return new List<HistoryParticipationsUserNewModel>(); }

            var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
            .Where(p => p.Photo.idUser == idPhotographer && p.Competition.resultsDate<=timeNow)
            .ToList();

            var grouped = participations
        .GroupBy(p => new
        {
            p.Competition.idCompetition,
            p.Competition.name,
            p.Competition.startDate,
            p.Competition.endDate
        })
        .Select(compGroup => new HistoryParticipationsUserNewModel
        {
            idCompetition = compGroup.Key.idCompetition,
            competitionName = compGroup.Key.name,
            startDate = compGroup.Key.startDate,
            endDate = compGroup.Key.endDate,
            sectionsDetails = compGroup
            .GroupBy(p => p.Section.name)
            .Select(secGroup => new SectionCompDetailModel
            {
                sectionName = secGroup.Key,
                photoDetails = secGroup.Select(p => new PhotoSectionDetailModel
                {
                    idPhoto = p.idPhoto,
                    photoUrl = p.Photo.photoUrl,
                    totalPoints = p.totalPoints,
                    ranking = p.ranking,
                    finalResult = p.finalResult
                }).ToList()
            }).ToList()
        })
        .OrderByDescending(h => h.startDate)
        .ThenBy(h => h.competitionName)
        .ToList();

            return grouped;
        }

        public List<BestOfModel> GetBestOfSectionByPh(string email)
        {
            var currentDay = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (idPhotographer == null) { return new List<BestOfModel>(); }

            var bestOf = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.Photo.idUser == idPhotographer
                && p.Competition.resultsDate <= currentDay)
                .GroupBy(p => p.Section.name)
                .Select(p => new BestOfModel
                {
                    sectionName = p.Key,
                    idCompetition = p.Where(points => points.finalResult == p.Max(po => po.finalResult))
                                      .FirstOrDefault().Competition.idCompetition,
                    competitionName = p.Where(points => points.finalResult == p.Max(po => po.finalResult))
                                      .FirstOrDefault().Competition.name,
                    idPhoto = p.Where(points => points.finalResult == p.Max(po => po.finalResult))
                                      .FirstOrDefault().Photo.idPhoto,
                    photoUrl = p.Where(points => points.finalResult == p.Max(po => po.finalResult))
                                      .FirstOrDefault().Photo.photoUrl,
                    startDate = p.Where(points => points.finalResult == p.Max(po => po.finalResult))
                                      .FirstOrDefault().Competition.startDate,
                    endDate = p.Where(points => points.finalResult == p.Max(po => po.finalResult))
                                      .FirstOrDefault().Competition.endDate,
                    ranking = p.Where(points => points.finalResult == p.Max(po => po.finalResult))
                                    .FirstOrDefault().ranking,
                    finalResult = p.Max(points => points.finalResult),
                   
                })
                .ToList();
            return bestOf;
        }

        public List<ParticipationPhotoRankModel> GetPhParticipationsCompNationSection(int idComp, int idSection, string nation="all nations")
        {
            var section = sectionRepo.GetSectionsIQueryable().FirstOrDefault(s => s.idSection == idSection);
            var competition = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => c.idCompetition == idComp)
                .FirstOrDefault();
            var timeNow = DateTime.Now;
            if (timeNow < competition.resultsDate)
            {
                if (nation == "all nations")
                {
                    var results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                        .Where(p => p.idCompetition == idComp && p.idSection == idSection)
                        .Select(p => new ParticipationPhotoRankModel
                        {
                            idPhoto = p.idPhoto,
                            photoUrl = p.Photo.photoUrl,
                            points = p.totalPoints,
                            finalResult = p.finalResult,
                            ranking = p.ranking,
                            sectionName = section.name,
                            idUser = p.Photo.idUser,
                            photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                            profileUrl = p.Photo.Photographer.profilePicture,
                            emailPhotographer = p.Photo.Photographer.Email,
                        })
                        .OrderBy(p => p.sectionName)
                        .ThenByDescending(p => p.points)
                        .ToList();
                    return results;
                }
                else
                {
                    var results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                       .Where(p => p.idCompetition == idComp && p.idSection == idSection && p.Photo.Photographer.Nationality.name == nation)
                       .Select(p => new ParticipationPhotoRankModel
                       {
                           idPhoto = p.idPhoto,
                           photoUrl = p.Photo.photoUrl,
                           points = p.totalPoints,
                           finalResult = p.finalResult,
                           ranking = p.ranking,
                           sectionName = section.name,
                           idUser = p.Photo.idUser,
                           photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                           profileUrl = p.Photo.Photographer.profilePicture,
                           emailPhotographer = p.Photo.Photographer.Email
                       })
                       .OrderBy(p => p.sectionName)
                       .ThenByDescending(p => p.points)
                       .ToList();
                    return results;
                }
            }
            else
            {
                if (nation == "all nations")
                {
                    var results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                        .Where(p => p.idCompetition == idComp && p.idSection == idSection && p.ranking!=0)
                        .Select(p => new ParticipationPhotoRankModel
                        {
                            idPhoto = p.idPhoto,
                            photoUrl = p.Photo.photoUrl,
                            points = p.totalPoints,
                            finalResult = p.finalResult,
                            ranking = p.ranking,
                            sectionName = section.name,
                            idUser = p.Photo.idUser,
                            photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                            profileUrl = p.Photo.Photographer.profilePicture,
                            emailPhotographer = p.Photo.Photographer.Email,
                        })
                        .OrderBy(p => p.sectionName)
                        .ThenByDescending(p => p.finalResult)
                        .ToList();
                    return results;
                }
                else
                {
                    var results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                       .Where(p => p.idCompetition == idComp && p.idSection == idSection && p.Photo.Photographer.Nationality.name == nation && p.ranking != 0)
                       .Select(p => new ParticipationPhotoRankModel
                       {
                           idPhoto = p.idPhoto,
                           photoUrl = p.Photo.photoUrl,
                           points = p.totalPoints,
                           finalResult = p.finalResult,
                           ranking = p.ranking,
                           sectionName = section.name,
                           idUser = p.Photo.idUser,
                           photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                           profileUrl = p.Photo.Photographer.profilePicture,
                           emailPhotographer = p.Photo.Photographer.Email
                       })
                       .OrderBy(p => p.sectionName)
                       .ThenByDescending(p => p.finalResult)
                       .ToList();
                    return results;
                }
            }
        }

        public List<ParticipationPhotoRankModel> GetParticipationByComp(int idComp, string firstName, string lastName)
        {
            var results = new List<ParticipationPhotoRankModel>();
            var timeNow = DateTime.Now;
            var competition = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == idComp).FirstOrDefault();
            if (timeNow < competition.resultsDate)
            {

                if (firstName == "null" && lastName == "null")
                {
                    results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                        .Where(p => p.idCompetition == idComp)
                        .Select(p => new ParticipationPhotoRankModel
                        {
                            idPhoto = p.idPhoto,
                            photoUrl = p.Photo.photoUrl,
                            points = p.totalPoints,
                            finalResult = p.finalResult,
                            ranking = p.ranking,
                            sectionName = p.Section.name,
                            idUser = p.Photo.idUser,
                            photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                            profileUrl = p.Photo.Photographer.profilePicture,
                            emailPhotographer = p.Photo.Photographer.Email
                        })
                       .OrderBy(p => p.sectionName)
                       .ThenByDescending(p => p.points)
                       .ToList();
                    return results;
                }
                else if (firstName != "null" && lastName == "null")
                {
                    results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                       .Where(p => p.idCompetition == idComp && p.Photo.Photographer.firstName.Contains(firstName))
                       .Select(p => new ParticipationPhotoRankModel
                       {
                           idPhoto = p.idPhoto,
                           photoUrl = p.Photo.photoUrl,
                           points = p.totalPoints,
                           ranking = p.ranking,
                           sectionName = p.Section.name,
                           finalResult = p.finalResult,
                           idUser = p.Photo.idUser,
                           photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                           profileUrl = p.Photo.Photographer.profilePicture,
                           emailPhotographer = p.Photo.Photographer.Email
                       })
                      .OrderBy(p => p.sectionName)
                      .ThenByDescending(p => p.points)
                      .ToList();
                    
                    return results; 
                }
                else if (firstName == "null" && lastName != "null")
                {
                    results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                       .Where(p => p.idCompetition == idComp && p.Photo.Photographer.lastName.Contains(lastName))
                       .Select(p => new ParticipationPhotoRankModel
                       {
                           idPhoto = p.idPhoto,
                           photoUrl = p.Photo.photoUrl,
                           points = p.totalPoints,
                           finalResult = p.finalResult,
                           ranking = p.ranking,
                           sectionName = p.Section.name,
                           idUser = p.Photo.idUser,
                           photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                           profileUrl = p.Photo.Photographer.profilePicture,
                           emailPhotographer = p.Photo.Photographer.Email
                       })
                      .OrderBy(p => p.sectionName)
                      .ThenByDescending(p => p.points)
                      .ToList();
                    return results;
                }
                else
                {
                    results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                       .Where(p => p.idCompetition == idComp
                       && p.Photo.Photographer.firstName.Contains(firstName)
                       && p.Photo.Photographer.lastName.Contains(lastName))
                       .Select(p => new ParticipationPhotoRankModel
                       {
                           idPhoto = p.idPhoto,
                           photoUrl = p.Photo.photoUrl,
                           points = p.totalPoints,
                           ranking = p.ranking,
                           sectionName = p.Section.name,
                           finalResult = p.finalResult,
                           idUser = p.Photo.idUser,
                           photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                           profileUrl = p.Photo.Photographer.profilePicture,
                           emailPhotographer = p.Photo.Photographer.Email
                       })
                      .OrderBy(p => p.sectionName)
                      .ThenByDescending(p => p.points)
                      .ToList();
                    return results;
                }
            }
            else
            {
                if (firstName == "null" && lastName == "null")
                {
                    results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                        .Where(p => p.idCompetition == idComp && p.ranking!=0)
                        .Select(p => new ParticipationPhotoRankModel
                        {
                            idPhoto = p.idPhoto,
                            photoUrl = p.Photo.photoUrl,
                            points = p.totalPoints,
                            ranking = p.ranking,
                            sectionName = p.Section.name,
                            finalResult = p.finalResult,
                            idUser = p.Photo.idUser,
                            photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                            profileUrl = p.Photo.Photographer.profilePicture,
                            emailPhotographer = p.Photo.Photographer.Email
                        })
                       .OrderBy(p => p.sectionName)
                       .ThenByDescending(p => p.finalResult)
                       .ToList();
                    return results;
                }
                else if (firstName != "null" && lastName == "null")
                {
                    results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                       .Where(p => p.idCompetition == idComp && p.Photo.Photographer.firstName.Contains(firstName) && p.ranking != 0)
                       .Select(p => new ParticipationPhotoRankModel
                       {
                           idPhoto = p.idPhoto,
                           photoUrl = p.Photo.photoUrl,
                           points = p.totalPoints,
                           ranking = p.ranking,
                           sectionName = p.Section.name,
                           finalResult = p.finalResult,
                           idUser = p.Photo.idUser,
                           photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                           profileUrl = p.Photo.Photographer.profilePicture,
                           emailPhotographer = p.Photo.Photographer.Email
                       })
                      .OrderBy(p => p.sectionName)
                      .ThenByDescending(p => p.finalResult)
                      .ToList();
                    return results;
                }
                else if (firstName == "null" && lastName != "null")
                {
                    results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                       .Where(p => p.idCompetition == idComp && p.Photo.Photographer.lastName.Contains(lastName) && p.ranking != 0)
                       .Select(p => new ParticipationPhotoRankModel
                       {
                           idPhoto = p.idPhoto,
                           photoUrl = p.Photo.photoUrl,
                           points = p.totalPoints,
                           ranking = p.ranking,
                           sectionName = p.Section.name,
                           finalResult = p.finalResult,
                           idUser = p.Photo.idUser,
                           photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                           profileUrl = p.Photo.Photographer.profilePicture,
                           emailPhotographer = p.Photo.Photographer.Email
                       })
                      .OrderBy(p => p.sectionName)
                      .ThenByDescending(p => p.finalResult)
                      .ToList();
                    return results;
                }
                else
                {
                    results = participationRepo.GetParticipationsSectionCompetitionPhoto()
                       .Where(p => p.idCompetition == idComp
                       && p.Photo.Photographer.firstName.Contains(firstName)
                       && p.Photo.Photographer.lastName.Contains(lastName) && p.ranking!=0)
                       .Select(p => new ParticipationPhotoRankModel
                       {
                           idPhoto = p.idPhoto,
                           photoUrl = p.Photo.photoUrl,
                           points = p.totalPoints,
                           ranking = p.ranking,
                           sectionName = p.Section.name,
                           finalResult = p.finalResult,
                           idUser = p.Photo.idUser,
                           photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                           profileUrl = p.Photo.Photographer.profilePicture,
                           emailPhotographer = p.Photo.Photographer.Email
                       })
                      .OrderBy(p => p.sectionName)
                      .ThenByDescending(p => p.finalResult)
                      .ToList();
                    return results;
                }
            }
        }


        //pentru toate competitiile active ale userului dat sa se afiseze rankingul si punctele publicului per fiecare participare ordonate dupa sectiune 
        //public List<ParticipationPhotoRankModel> GetParticipationByComp(int idComp, string firstName, string lastName)

        /*        public List<ActiveCompetitionsUser> GetAllActiveCompetitionsUser(string email)
                {
                    var currentDate = DateTime.Now;
                    var users = userManager.Users;
                    var idPhotographer = users
                        .Where(u => u.Email.Equals(email))
                        .Select(u => u.Id)
                        .FirstOrDefault();
                    if (idPhotographer == null)
                    {
                        return new List<ActiveCompetitionsUser>();
                    }

                    var result = participationRepo.GetParticipationsSectionCompetitionPhoto()
                        .Where(p => p.Photo.idUser == idPhotographer &&
                                    p.Competition.startDate <= currentDate &&
                                    p.Competition.endDate >= currentDate)
                        .Select(p => new ActiveCompetitionsUser
                        {
                            competition = p.Competition.name,
                            endDate = p.Competition.endDate,
                            photoUrl = p.Photo.photoUrl,
                            ranking = p.ranking,
                            totalPoints = p.totalPoints,
                            sectionName = p.Section.name
                        })
                        .OrderBy(p => p.competition)
                        .ToList();

                    return result;
                }*/
        public List<CompetitionSectionsModel> GetAllActiveCompetitionsUser(string email)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (idPhotographer == null)
            {
                return new List<CompetitionSectionsModel>();
            }

            var result = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => c.Participations.Any(p => p.Photo.idUser == idPhotographer) &&
                            c.startDate <= currentDate &&
                            c.endDate >= currentDate)
                .OrderByDescending(c => c.endDate)
                .Select(c => new CompetitionSectionsModel
                {
                    competitionName = c.name,
                    endDate = c.endDate,
                    sections = c.Participations
                .Where(p => p.Photo.idUser == idPhotographer &&
                       p.Competition.startDate <= currentDate &&
                       p.Competition.endDate >= currentDate)
                .GroupBy(p => p.Section) 
                .Select(g => new SectionDetailModel
                {
                    sectionName = g.Key.name,
                    rankings = g.Select(p => new PhotoRankingModel
                    {
                        photoId = p.idPhoto,
                        photoUrl = p.Photo.photoUrl,
                        ranking = p.ranking,
                        totalPoints = p.totalPoints
                    }).ToList()
                })
                .OrderBy(s => s.sectionName) 
                .ToList()
                })
        .ToList();

            return result;
        }
        private string ExtractCompetitionName(string name)
        {
            return Regex.Replace(name, @"\s\d+$", ""); //removing the year from the name
        }
        public WinnerPhotoModel? GetPhotoSectionWinnerPreviousComp(int idSection, int idCompetition)
        {
            var competition = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == idCompetition).FirstOrDefault();
            var competitionNameExtract = ExtractCompetitionName(competition.name);
            //we need the winner photo of the last competition that has this section
            //comp is a list of Participation that has the same name as the new competition
            var comp = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => p.Section.idSection == idSection && ExtractCompetitionName(p.Competition.name).Equals(competitionNameExtract))
                .OrderByDescending(p => p.Competition.startDate)
                .ToList();
            if (comp.Count == 1) //this is the first edition of that competition
                return null;
            else
            {//if is not the first edition then the last edition is the second one in vector
                var winner = new WinnerPhotoModel
                {
                    photoUrl = comp[1].Photo.photoUrl,
                    userName = comp[1].Photo.Photographer.firstName + " " + comp[1].Photo.Photographer.lastName
                };
                return winner;
            }           
        }

        public List<ParticipationsForVotingModel> GetParticipationsForVoting(int idSection, int idCompetition)
        {
            var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idSection == idSection && p.idCompetition == idCompetition)
                .Select(ph => new ParticipationsForVotingModel
                {
                    idPhoto = ph.idPhoto,
                    idCompetition = ph.idCompetition,
                    idSection = ph.idSection,
                    photoUrl = ph.Photo.photoUrl
                })
                .ToList();
            return participations;
        }

        public List<ActiveSectionCompetitionModel> GetActiveSectionsCompetition(int idCompetition)
        {
            var sectionsActive = participationRepo.GetParticipationsSectionCompetitionPhoto() 
                .Where(p => p.idCompetition == idCompetition)
                .Select(p => p.idSection)
                .Distinct()
                .ToList();
            var sections = new List<ActiveSectionCompetitionModel>();
            for(int index=0; index<sectionsActive.Count; index++)
            {
                var section = sectionRepo.GetSectionsIQueryable()
                    .Where(s => s.idSection == sectionsActive[index])
                    .Select(s => new ActiveSectionCompetitionModel
                    {
                        idSection = s.idSection,
                        name = s.name,
                        participantsNr = participationRepo.GetParticipationsSectionCompetition().Where(p=>p.idCompetition == idCompetition).Count(p => p.idSection == s.idSection)

                    }).FirstOrDefault();

                if (section != null) 
                {
                    sections.Add(section);
                }
               
            }
            return sections;
        }

        public List<WinnerModel> GetWinners(int idCompetition, int idSection)
        {
            var currentDate = DateTime.Now;
            var competition = competitionRepo.GetCompetitionsIQueryable().FirstOrDefault(c => c.idCompetition == idCompetition);
            var winners = new List<WinnerModel>();
            if(currentDate >= competition.resultsDate)
            {
                 winners = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idCompetition && p.idSection == idSection && new[] { 1, 2, 3 }.Contains(p.ranking))
                .Select(p => new WinnerModel
                {
                    idPhoto = p.idPhoto,
                    totalPoints = p.totalPoints,
                    finalResult = p.finalResult,
                    ranking = p.ranking,
                    photoUrl = p.Photo.photoUrl,
                    idUser = p.Photo.idUser,
                    photographerName = p.Photo.Photographer.firstName + " " + p.Photo.Photographer.lastName,
                    profileUrl = p.Photo.Photographer.profilePicture,
                    emailPhotographer = p.Photo.Photographer.Email,
                    title = p.Photo.title,
                    takenDate = p.Photo.takenDate                    
                })
                .OrderBy(p => p.ranking).ToList();
            }
            
            return winners;
        }

        public List<WinnerModel> GetWinnersBeforeResults(int idCompetition, int idSection)
        {
            var currentDate = DateTime.Now;
            var competition = competitionRepo.GetCompetitionsIQueryable().FirstOrDefault(c => c.idCompetition == idCompetition);
            var winners = new List<WinnerModel>();
            if (currentDate < competition.resultsDate)
            {
                winners = participationRepo.GetParticipationsSectionCompetitionPhoto()
               .Where(p => p.idCompetition == idCompetition && p.idSection == idSection && new[] { 1, 2, 3 }.Contains(p.ranking))
               .Select(p => new WinnerModel
               {
                   idPhoto = p.idPhoto,
                   totalPoints = p.totalPoints,
                   finalResult = p.finalResult,
                   ranking = p.ranking,
                   photoUrl = p.Photo.photoUrl,
                   idUser = p.Photo.idUser,
                   photographerName = p.Photo.Photographer.firstName +" "+ p.Photo.Photographer.lastName,
                   profileUrl = p.Photo.Photographer.profilePicture,
                   emailPhotographer = p.Photo.Photographer.Email,
                   title = p.Photo.title,
                   takenDate = p.Photo.takenDate
               })
               .OrderBy(p => p.ranking).ToList();
            }

            return winners;
        }

        public List<ParticipationActiveJurorModel> GetParticipationActiveJuror(int idCompetition, int idSection)
        {
            var participation = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idCompetition && p.idSection == idSection)
                .Select(p => new ParticipationActiveJurorModel
                {
                    idPhoto = p.idPhoto,
                    idCompetition = p.idCompetition,
                    idSection = p.idSection,
                    photoUrl = p.Photo.photoUrl
                })
                .ToList();
            return participation;
        }

        public double GetNoPodiumCompStatistic(string emailPh)
        {
            var users = userManager.Users;
            var currentDate = DateTime.Now;
            var idPhotographer = users
                .Where(u => u.Email.Equals(emailPh))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (idPhotographer == null)
            {
                return -2;
            }

            var noTotalPartComp = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => p.Photo.Photographer.Id == idPhotographer && currentDate >= p.Competition.resultsDate)
                .Select(p => p.idCompetition) 
                .Distinct()                   
                .Count();

            var noPodiumPartComp = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => p.Photo.Photographer.Id == idPhotographer && p.ranking >=1 && p.ranking<=3 && currentDate >= p.Competition.resultsDate)
                .Select(p => p.idCompetition)
                .Distinct()
                .Count();
            if (noTotalPartComp == 0)
            {
                return 0;//-1
            }
            double percentage = ((double)noPodiumPartComp * 100) / noTotalPartComp;
            return Math.Round(percentage, 2);
        }

        public List<CompetitionStatisticsPhModel> GetCompYearSectPhStatistic(string emailPh, string year_, string section)
        {
            var users = userManager.Users;
            var currentDate = DateTime.Now;
            var idPhotographer = users
                .Where(u => u.Email.Equals(emailPh))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (idPhotographer == null)
            {
                return new List<CompetitionStatisticsPhModel>();
            }

            var participations = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => p.Photo.Photographer.Id == idPhotographer && currentDate >= p.Competition.resultsDate);

           
           if(year_ != "allYears")
            {
                //participations = participations.Where(p => p.Competition.resultsDate.Year == int.Parse(year_));
                participations = participations.Where(p => p.Competition.startDate.Year == int.Parse(year_));
            }

           if(section != "allSections")
            {
                participations = participations.Where(p => p.Section.name == section);
            }

            var groupComp = participations.AsEnumerable().GroupBy(p => new { competition = p.Competition.name, section = p.Section.name })
                 .Select(g => new
                 {
                     competitionName = g.Key.competition,
                     sectionName = g.Key.section,
                     totalPodium = g.Count(p => p.ranking >= 1 && p.ranking <= 3),
                     rankings = g.Where(p => p.ranking >= 1 && p.ranking <= 3)
                         .OrderBy(p => p.ranking)
                         .Select(p => p.ranking)
                         .ToList()

                 });

            var result = groupComp
            .GroupBy(g => g.competitionName)
            .Select(cg => new CompetitionStatisticsPhModel
            {
                competitionName = cg.Key,
                sectionDetails = cg.Select(s => new SectionStatisticPhModel
                {
                    sectionName = s.sectionName,
                    totalPodium = s.totalPodium,
                    rankings = s.rankings
                }).ToList()
            })
            .ToList();

            return result;
        }

        public double GetDeniedCompJStatistic(string emailJur, string compName)
        {
            var users = userManager.Users;
            var currentDate = DateTime.Now;
            var idJuror = users
                .Where(u => u.Email.Equals(emailJur))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (idJuror == null)
            {
                return -2;
            }

            if (compName.Equals("allComp"))
            {
                //numar total de participari din concursuri jurizate de juror

                var competitions = assignementRepo.GetAssignementsIQueryable()
                .Where(a => a.idUser == idJuror && currentDate >= a.Competition.resultsDate)
                .Select(a => a.idCompetition)
                .ToList();

                int noTotalPart_ = 0;
                int noDeniedPart_ = 0;
                foreach (var comp in competitions)
                {
                    noTotalPart_ = noTotalPart_ + participationRepo.GetParticipationsSectionCompetition()
                                    .Where(p => p.idCompetition == comp && currentDate >= p.Competition.resultsDate)
                                    .Count();

                    noDeniedPart_ = noDeniedPart_ + participationRepo.GetParticipationsSectionCompetition()
                                .Where(p => p.idCompetition == comp && currentDate >= p.Competition.resultsDate && p.reportMessage == "DENIED")
                                .Count();
                }
                if (noTotalPart_ == 0)
                {
                    return 0;
                }
                double percentage_ = ((double)noDeniedPart_ * 100) / noTotalPart_;
                return Math.Round(percentage_, 2);
            }


            var competition = competitionRepo.GetCompetitionsIQueryable().Where(c => c.name == compName).FirstOrDefault();
            if (competition == null)
            {
                return -2;
            }

            var noTotalPart = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => p.idCompetition == competition.idCompetition && currentDate >= p.Competition.resultsDate)
                .Count();

            var noDeniedPart = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => p.idCompetition == competition.idCompetition && currentDate >= p.Competition.resultsDate && p.reportMessage == "DENIED")
                .Count();

        
            if (noTotalPart == 0)
            {
                return 0;
            }
            double percentage = ((double)noDeniedPart * 100) / noTotalPart;
            return Math.Round(percentage, 2);
        }
    }
}
