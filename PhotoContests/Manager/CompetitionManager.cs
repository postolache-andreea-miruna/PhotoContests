using PhotoContests.Repo;
using PhotoContests.Models;
using PhotoContests.Entities;
using Microsoft.AspNetCore.Identity;
using PhotoContests.Migrations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PhotoContests.Manager
{
    public class CompetitionManager: ICompetitionManager
    {
        private readonly ICompetitionRepo competitionRepo;
        private readonly UserManager<User> userManager;
        private readonly IParticipationRepo participationRepo;
        private readonly IAssignementRepo assignementRepo;
        private readonly IVideoRepo videoRepo;
        public CompetitionManager(ICompetitionRepo competitionRepo,
            UserManager<User> userManager,
            IParticipationRepo participationRepo,
            IAssignementRepo assignementRepo,
            IVideoRepo videoRepo)
        {
            this.competitionRepo = competitionRepo;
            this.userManager = userManager;
            this.participationRepo = participationRepo;
            this.assignementRepo = assignementRepo;
            this.videoRepo = videoRepo;
        }

        public void Create(CompetitionCreateModel model)
        {
            var newCompetition = new Competition
            {
                name = model.name,
                description = model.description,
                startDate = model.startDate,
                endDate = model.endDate,
                resultsDate = model.resultsDate,
                urlPosterPhoto = model.urlPosterPhoto,
                photosLimit = model.photosLimit,
                status = true,
                idType = model.idType,
                datePhotoTakenLimit = model.datePhotoTakenLimit
            };

            competitionRepo.Create(newCompetition);
        }


        public void Update(CompetitionUpdateModel model)
        {
            var competition = competitionRepo.GetCompetitionsIQueryable()
                .FirstOrDefault(c => c.idCompetition == model.idCompetition);
            if (competition == null) return;
            competition.name= model.name;
            competition.description= model.description;
            competition.startDate= model.startDate;
            competition.endDate= model.endDate; 
            competition.resultsDate= model.resultsDate;
            competition.urlPosterPhoto= model.urlPosterPhoto;
            competition.photosLimit= model.photosLimit;
            competition.status= model.status;
            competition.idType= model.idType;
            competitionRepo.Update(competition);
        }

        /*public void Delete(int id)
        {
            var competition = competitionRepo.GetCompetitionsIQueryable()
                .FirstOrDefault(c => c.idCompetition == id);
            if(competition == null) return;
            competition.status = false; //competition was canceled
            competitionRepo.Update(competition);
        }*/
        public List<CompetitionGetAllInfoModel?> GetAllInfoCompetitions()
        {
            var competitions = competitionRepo.GetCompetitionsIQueryable();
            if (competitions == null)
            {
                return new List<CompetitionGetAllInfoModel>();
            }
            var models = competitions
                .Select(c => new CompetitionGetAllInfoModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    description = c.description,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    resultsDate = c.resultsDate,                
                    datePhotoTakenLimit = c.datePhotoTakenLimit,
                    urlPosterPhoto = c.urlPosterPhoto,
                    photosLimit = c.photosLimit,
                    status = c.status,
                    nameType = c.Type.competitionType,
                    idType = c.idType
                })
                .OrderByDescending(c => c.startDate)
                .ToList();
            return models;
        }

        public List<CompetitionGetAllModel?> GetAllCompetitions()
        {
            var competitions = competitionRepo.GetCompetitionsIQueryable();
            if (competitions == null)
            {
                return new List<CompetitionGetAllModel>();
            }
            var models = competitions
                .Select(c => new CompetitionGetAllModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    status = c.status,
                    urlPosterPhoto = c.urlPosterPhoto,
                    nameType = c.Type.competitionType,
                    photosLimit= c.photosLimit
                })
                .OrderByDescending(c => c.startDate)
                .ToList();
            return models;
        }


        public List<CompetitionGetAllModel?> GetAllCompetitionsCurrentYear()
        {
            var competitions = competitionRepo.GetCompetitionsIQueryable();
            if (competitions == null)
            {
                return new List<CompetitionGetAllModel>();
            }
            var models = competitions
                .Where(c => c.startDate.Year == DateTime.Now.Year)
                .Select(c => new CompetitionGetAllModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    status = c.status,
                    urlPosterPhoto = c.urlPosterPhoto,
                    nameType = c.Type.competitionType,
                    photosLimit= c.photosLimit
                })
                .OrderByDescending(c => c.startDate)
                .ToList();
            return models;
        }

        public List<CompetitionGetAllModel?> GetAllCompetitionsByYear(int year)
        {
            var competitions = competitionRepo.GetCompetitionsIQueryable();
            if (competitions == null)
            {
                return new List<CompetitionGetAllModel>();
            }
            var models = competitions
                .Where(c => c.startDate.Year == year)
                .Select(c => new CompetitionGetAllModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    status = c.status,
                    urlPosterPhoto = c.urlPosterPhoto,
                    nameType = c.Type.competitionType,
                    photosLimit= c.photosLimit
                })
                .OrderByDescending(c => c.startDate)
                .ToList();
            return models;
        }

        /*public List<CompetitionGetAllModel?> GetAllActiveCompetitions()
        {
            var competitions = competitionRepo.GetCompetitionsIQueryable();
            if (competitions == null)
            {
                return new List<CompetitionGetAllModel>();
            }
            var models = competitions
                .Where(c => c.status == true)
                .Select(c => new CompetitionGetAllModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    status = c.status,
                    urlPosterPhoto = c.urlPosterPhoto,
                    nameType = c.Type.competitionType,
                    photosLimit= c.photosLimit
                })
                .OrderByDescending(c => c.startDate)
                .ToList();
            return models;
        }*/
        public List<CompetitionGetAllParticpModel> GetAllActiveCompetitions()
        {
            var competitions = competitionRepo.GetCompetitionsIQueryable();
            if (competitions == null)
            {
                return new List<CompetitionGetAllParticpModel>();
            }
            var models = competitions
                .Where(c => c.status == true)
                .Select(c => new CompetitionGetAllParticpModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    status = c.status,
                    urlPosterPhoto = c.urlPosterPhoto,
                    nameType = c.Type.competitionType,
                    photosLimit = c.photosLimit,
                    photoParticipants = participationRepo.GetParticipationsSectionCompetition().Where(p => p.idCompetition == c.idCompetition).Count()
                })
                .OrderByDescending(c => c.startDate)
                .ToList();
            return models;
        }
        public List<CompetitionGetAllParticpModel> GetAllActiveCurrentCompetitions()
        {
            var currentDate = DateTime.Now;
            var competitions = competitionRepo.GetCompetitionsIQueryable();
            if (competitions == null)
            {
                return new List<CompetitionGetAllParticpModel>();
            }
            var models = competitions
                .Where(c => c.status == true && c.startDate<=currentDate && c.endDate>=currentDate)
                .Select(c => new CompetitionGetAllParticpModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    status = c.status,
                    urlPosterPhoto = c.urlPosterPhoto,
                    nameType = c.Type.competitionType,
                    photosLimit = c.photosLimit,
                    photoParticipants = participationRepo.GetParticipationsSectionCompetition().Where(p => p.idCompetition == c.idCompetition).Count()
                })
                .OrderByDescending(c => c.endDate)
                .ToList();
            return models;
        }



        public List<CompetitionByIdModel>GetCompetitionById(int id)
        {
            var competition = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => c.idCompetition == id)
                .Select(c => new CompetitionByIdModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    description = c.description,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    resultsDate = c.resultsDate,
                    urlPosterPhoto = c.urlPosterPhoto,
                    photosLimit = c.photosLimit,
                    status = c.status,
                    nameType = c.Type.competitionType
                })
            .ToList();
            return competition;
        }

        public List<CompetitionGetAllModel?> GetAllCompetitionsByPhotographer(string email)
        {
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();

            if(idPhotographer == null) { return new List<CompetitionGetAllModel?>(); }

            var competitions = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.Photo.idUser == idPhotographer)
                .Select(p => new CompetitionGetAllModel
                {
                    idCompetition = p.idCompetition,
                    name = p.Competition.name,
                    startDate = p.Competition.startDate,
                    endDate = p.Competition.endDate,
                    status = p.Competition.status,
                    urlPosterPhoto = p.Competition.urlPosterPhoto,
                    nameType = p.Competition.Type.competitionType,
                    photosLimit= p.Competition.photosLimit,
                })
                .OrderByDescending(p => p.startDate)
                .ToList();
            return competitions;
        }

        public List<CompetitionNameModel> GetAllCompetitionsNamesByPhotographer(string email)
        {
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            var currentDate = DateTime.Now;
            if (idPhotographer == null) { return new List<CompetitionNameModel>(); }
            var competitions = videoRepo.GetVideosIQueryable()
                .Where(v => v.Participation.Photo.idUser == idPhotographer 
                && v.Participation.Competition.resultsDate <= currentDate)
                .Select(v => new CompetitionNameModel
                {
                    name = v.Participation.Competition.name
                })
                .Distinct()
                .OrderBy(p => p.name)
                .ToList();
            /* var competitions = participationRepo.GetParticipationsSectionCompetitionPhoto()
                 .Where(p => p.Photo.idUser == idPhotographer)
                 .Select(p => new CompetitionNameModel
                 {
                     name = p.Competition.name,                
                 })
                 .Distinct()
                 .OrderBy(p => p.name)
                 .ToList();*/
            return competitions;
        }

        //active competition that a user wasn't registered yet
        public List<AllCompetitionActiveModel> GetAllCompetitionsActive(string email)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idPhotographer == null) { return new List<AllCompetitionActiveModel>(); }

            var competitionsActiveNoParticipate = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => c.startDate <= currentDate && currentDate <= c.endDate && c.status == true &&
                           !c.Participations.Any(p => p.Photo.idUser == idPhotographer))
                .Select(c => new AllCompetitionActiveModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    endDate = c.endDate,
                    startDate = c.startDate,
                    nameType = c.Type.competitionType,
                    photosLimit = c.photosLimit,
                    urlPosterPhoto = c.urlPosterPhoto,
                    //// numarul de utilizatori distincti care sunt inscrisi la acest concurs
                    noParticipants = c.Participations
                                      .Select(p => p.Photo.idUser)
                                      .Distinct()
                                      .Count(),
                    photoParticipants = participationRepo.GetParticipationsSectionCompetition().Where(p => p.idCompetition == c.idCompetition).Count()
                })
                .OrderByDescending(c => c.endDate) 
                .ToList();
            return competitionsActiveNoParticipate;
        }

        //completed competitions by user
        public List<AllCompetitionCompleteModel> GetAllCompetitionsCompleted(string email)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idPhotographer == null) { return new List<AllCompetitionCompleteModel>(); }

            var competitionsComplete = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => currentDate > c.endDate && c.status == true && c.Participations.Any(p => p.Photo.idUser == idPhotographer))
                .Select(c => new AllCompetitionCompleteModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    urlPosterPhoto = c.urlPosterPhoto,
                    //// numarul de utilizatori distincti care sunt inscrisi la acest concurs
                    noParticipants = c.Participations
                                      .Select(p => p.Photo.idUser)
                                      .Distinct()
                                      .Count()
                })
                .OrderByDescending(c => c.endDate)
                .ToList();
            return competitionsComplete;
        }
        public List<PhotogrCompetitionsInJudgingModel> GetAllUserCompetitionsInJudging(string email)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idPhotographer == null) { return new List<PhotogrCompetitionsInJudgingModel>(); }

            var competitionsInJudging = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => currentDate >= c.endDate && currentDate < c.resultsDate && c.status == true && c.Participations.Any(p => p.Photo.idUser == idPhotographer))
                .Select(c => new PhotogrCompetitionsInJudgingModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    urlPosterPhoto = c.urlPosterPhoto,
                    //// numarul de utilizatori distincti care sunt inscrisi la acest concurs
                    noParticipants = c.Participations
                                      .Select(p => p.Photo.idUser)
                                      .Distinct()
                                      .Count(),
                    resultsDate= c.resultsDate,
                    photosLimits = c.photosLimit
                })
                .OrderByDescending(c => c.resultsDate)
                .ToList();
            return competitionsInJudging;
        }


        public List<UserCompletedCompetitionsUsedModel> GetUserAllCompletedCompetitions(string email) //the one used
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idPhotographer == null) { return new List<UserCompletedCompetitionsUsedModel>(); }

            var competitionsComplete = competitionRepo.GetCompetitionsIQueryable()
                .Where(c =>  currentDate >= c.resultsDate && c.status == true && c.Participations.Any(p => p.Photo.idUser == idPhotographer))
                //.Where(c => currentDate <= c.resultsDate && c.status == true && c.Participations.Any(p => p.Photo.idUser == idPhotographer))//for testing when I don't have a completed competiton for anyone
                .Select(c => new UserCompletedCompetitionsUsedModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    urlPosterPhoto = c.urlPosterPhoto,
                    //// numarul de utilizatori distincti care sunt inscrisi la acest concurs
                    noParticipants = c.Participations
                                      .Select(p => p.Photo.idUser)
                                      .Distinct()
                                      .Count(),
                    resultsDate = c.resultsDate,
                    photosLimits = c.photosLimit,
                    bestRank = participationRepo.GetParticipationsSectionCompetition()
                                    .Where(p => p.idCompetition == c.idCompetition && p.Photo.idUser == idPhotographer && p.ranking!=0)
                                    .GroupBy(p => p.Section.name)
                                    .Select(g => new BestRankingPerSection
                                    {
                                        section = g.Key,
                                        rank = g.Min(p => p.ranking)
                                    })
                                    .ToList()
                })
                .OrderByDescending(c => c.resultsDate)
                .ToList();
            return competitionsComplete;
        }



        public List<AllCompetitionCompleteModel> GetAllCompetitionsFinished()
        {
            var currentDate = DateTime.Now;
            var competitionsComplete = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => currentDate > c.endDate && c.status == true)
                .Select(c => new AllCompetitionCompleteModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    urlPosterPhoto = c.urlPosterPhoto,
                    //// numarul de utilizatori distincti care sunt inscrisi la acest concurs
                    noParticipants = c.Participations
                                      .Select(p => p.Photo.idUser)
                                      .Distinct()
                                      .Count(),
                    photosLimits = c.photosLimit,
                    resultsDate= c.resultsDate

                })
                .OrderByDescending(c => c.endDate)
                .ToList();
            return competitionsComplete;
        }


        public List<AllCompetitionFutureModel> GetAllCompetitionsFuture()
        {
            var currentDate = DateTime.Now;
            var competitionsFuture = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => currentDate < c.startDate && c.status == true)
                .Select(c => new AllCompetitionFutureModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    urlPosterPhoto = c.urlPosterPhoto,
                    photosLimit= c.photosLimit
                })
                .OrderBy(c => c.startDate)
                .ToList();
            return competitionsFuture;
        }



        public List<UserAllActiveCompModel> GetUserAllActiveCompetitionsPhotos(string email) //the one used
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idPhotographer == null) { return new List<UserAllActiveCompModel>(); }


            var myParticipation = new List<UserAllActiveCompModel>();
            
            
            /////////////////////////
            var activeComp = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => currentDate <= p.Competition.endDate && currentDate >= p.Competition.startDate && p.Competition.status == true && p.Photo.idUser == idPhotographer)
                 /*.Select(p => new UserCompSectModel
                 {
                     idCompetition = p.idCompetition,
                     sections = participationRepo.GetParticipationsSectionCompetition()
                     .Where(pc => currentDate <= pc.Competition.endDate && currentDate >= pc.Competition.startDate && pc.Competition.status == true && pc.Photo.idUser == idPhotographer && pc.idCompetition == p.idCompetition)
                     .Select(pc => pc.idSection).Distinct().ToList()
                 })*/
                 .GroupBy(p => p.idCompetition)  
                .Select(group => new UserCompSectModel
                {
                    idCompetition = group.Key,  
                    sections = group.Select(p => p.idSection).Distinct().ToList()  
                })
                .ToList();
            
            for (int comp_ = 0; comp_ < activeComp.Count; comp_++)
            {
                myParticipation.Add(new UserAllActiveCompModel());

                myParticipation[comp_].idCompetition = activeComp[comp_].idCompetition;
                myParticipation[comp_].name = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == activeComp[comp_].idCompetition).First().name;
                myParticipation[comp_].endDate = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == activeComp[comp_].idCompetition).First().endDate;
                myParticipation[comp_].startDate = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == activeComp[comp_].idCompetition).First().startDate;
                myParticipation[comp_].urlPosterPhoto = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == activeComp[comp_].idCompetition).First().urlPosterPhoto;


                myParticipation[comp_].noParticipants = participationRepo.GetParticipationsSectionCompetitionPhoto()
                                                    .Where(p => p.idCompetition == activeComp[comp_].idCompetition)
                                                    .Select(p => p.Photo.idUser)
                                                    .Distinct()
                                                    .Count();
                myParticipation[comp_].photosLimits = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == activeComp[comp_].idCompetition).First().photosLimit;
                myParticipation[comp_].resultsDate = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == activeComp[comp_].idCompetition).First().resultsDate;
                myParticipation[comp_].sectionDetail = new List<UserAllActiveCompSectionModel>();

                for (int sect_=0; sect_ < activeComp[comp_].sections.Count; sect_++)
                {
                    myParticipation[comp_].sectionDetail.Add(new UserAllActiveCompSectionModel());

                    myParticipation[comp_].sectionDetail[sect_].idSection = activeComp[comp_].sections[sect_];
                    myParticipation[comp_].sectionDetail[sect_].sectionName = participationRepo.GetParticipationsSectionCompetition().Where(p => p.idSection == activeComp[comp_].sections[sect_]).Select(p => p.Section.name).FirstOrDefault();
                    

                    var participants_ = participationRepo.GetParticipationsSectionCompetitionPhoto()
                    .Where(p => p.idCompetition == activeComp[comp_].idCompetition && p.idSection == activeComp[comp_].sections[sect_])
                    .GroupBy(p => new
                    {
                        p.Photo.Photographer.firstName,
                        p.Photo.Photographer.lastName,
                        p.Photo.Photographer.Id,
                        p.Photo.Photographer.profilePicture
                    })
                    .Select(g => new
                    {
                        userId = g.Key.Id,
                        userName = g.Key.firstName + " " + g.Key.lastName,
                        profilePicture = g.Key.profilePicture,
                        totalPoints = g.Sum(p => p.totalPoints),
                        photos = g.Select(p => new UserAllActiveCompSectionPhotosModel
                        {
                            idPhoto = p.idPhoto,
                            urlPhoto = p.Photo.photoUrl,
                            totalPoints = p.totalPoints,
                            rankPhoto = p.ranking
                        }).ToList()
                    })
                    .OrderByDescending(p => p.totalPoints)
                    .ToList();

                        var rankedParticipants_ = participants_.Select((user, counter) => new
                        {
                            idUser = user.userId,
                            userName = user.userName,
                            totalPoints = user.totalPoints,
                            profilePicture = user.profilePicture,
                            ranking = counter + 1,
                            photos = user.photos
                        }).ToList();

                    for (int index = 0; index < rankedParticipants_.Count; index++)
                    {
                        if (rankedParticipants_[index].idUser == idPhotographer) //se afla rankul utilizatorului pentru acea sectiune
                        {
                            myParticipation[comp_].sectionDetail[sect_].rankingPerUser = rankedParticipants_[index].ranking;
                            myParticipation[comp_].sectionDetail[sect_].userTotalPoints = rankedParticipants_[index].totalPoints;
                            myParticipation[comp_].sectionDetail[sect_].photosDetails = rankedParticipants_[index].photos;
                        }
                    }
                    
                }
            }
            ///////////////////
               
            return myParticipation;
        }
        public List<AllCompetitionsActiveJurorModel> GetAllActiveCompetitionsJuror(string emailJuror)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idJuror = users
                .Where(u => u.Email.Equals(emailJuror))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idJuror == null) { return new List<AllCompetitionsActiveJurorModel>(); }

            var competitionsActive = assignementRepo.GetAssignementsIQueryable()
                .Where(a => a.idUser == idJuror && currentDate >= a.Competition.startDate && currentDate<= a.Competition.endDate && a.Competition.status == true)
                .Select(a => new AllCompetitionsActiveJurorModel
                {
                    idCompetition = a.idCompetition,
                    name = a.Competition.name,
                    endDate = a.Competition.endDate,
                    startDate = a.Competition.startDate,
                    nameType = a.Competition.Type.competitionType,
                    photosLimit = a.Competition.photosLimit,
                    urlPosterPhoto = a.Competition.urlPosterPhoto,
                    //// numarul de utilizatori distincti care sunt inscrisi la acest concurs
                    noParticipants = a.Competition.Participations
                                      .Select(p => p.Photo.idUser)
                                      .Distinct()
                                      .Count(),
                    photoParticipants = participationRepo.GetParticipationsSectionCompetition().Where(p => p.idCompetition == a.idCompetition).Count(),
                    resultDate = a.Competition.resultsDate
                })
                .OrderBy(c => c.endDate)
                .ToList();
            return competitionsActive;
        }


        public List<AllCompetitionsActiveJurorModel> GetAllEndedCompetitionsJuror(string emailJuror)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idJuror = users
                .Where(u => u.Email.Equals(emailJuror))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idJuror == null) { return new List<AllCompetitionsActiveJurorModel>(); }

            var competitions = assignementRepo.GetAssignementsIQueryable()
                .Where(a => a.idUser == idJuror && currentDate > a.Competition.endDate && currentDate < a.Competition.resultsDate && a.Competition.status == true)
                .Select(a => new AllCompetitionsActiveJurorModel
                {
                    idCompetition = a.idCompetition,
                    name = a.Competition.name,
                    endDate = a.Competition.endDate,
                    startDate = a.Competition.startDate,
                    nameType = a.Competition.Type.competitionType,
                    photosLimit = a.Competition.photosLimit,
                    urlPosterPhoto = a.Competition.urlPosterPhoto,
                    //// numarul de utilizatori distincti care sunt inscrisi la acest concurs
                    noParticipants = a.Competition.Participations
                                      .Select(p => p.Photo.idUser)
                                      .Distinct()
                                      .Count(),
                    photoParticipants = participationRepo.GetParticipationsSectionCompetition().Where(p => p.idCompetition == a.idCompetition).Count(),
                    resultDate = a.Competition.resultsDate
                })
                .OrderByDescending(c => c.endDate)
                .ToList();
            return competitions;
        }


        public List<AllCompetitionsActiveJurorModel> GetAllFinishedCompetitionsJuror(string emailJuror)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idJuror = users
                .Where(u => u.Email.Equals(emailJuror))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idJuror == null) { return new List<AllCompetitionsActiveJurorModel>(); }

            var competitions = assignementRepo.GetAssignementsIQueryable()
                .Where(a => a.idUser == idJuror && currentDate >=  a.Competition.resultsDate && a.Competition.status == true)
                .Select(a => new AllCompetitionsActiveJurorModel
                {
                    idCompetition = a.idCompetition,
                    name = a.Competition.name,
                    endDate = a.Competition.endDate,
                    startDate = a.Competition.startDate,
                    nameType = a.Competition.Type.competitionType,
                    photosLimit = a.Competition.photosLimit,
                    urlPosterPhoto = a.Competition.urlPosterPhoto,
                    //// numarul de utilizatori distincti care sunt inscrisi la acest concurs
                    noParticipants = a.Competition.Participations
                                      .Select(p => p.Photo.idUser)
                                      .Distinct()
                                      .Count(),
                    photoParticipants = participationRepo.GetParticipationsSectionCompetition().Where(p => p.idCompetition == a.idCompetition).Count(),
                    resultDate = a.Competition.resultsDate
                })
                .OrderByDescending(c => c.resultDate)
                .ToList();
            return competitions;
        }








        public List<AllCompetitionFutureModel> GetAllCompetitionsJuror(string emailJuror,string type="active")
        {
            var users = userManager.Users;
            var idJuror = users
                .Where(u => u.Email.Equals(emailJuror))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idJuror == null) { return new List<AllCompetitionFutureModel>(); }
            var currentDate = DateTime.Now;
            if (type == "active")
            {
                var competitionsActive = assignementRepo.GetAssignementsIQueryable()
                    .Where(a => a.idUser == idJuror && currentDate < a.Competition.endDate && a.Competition.status == true)
                    .Select(a => new AllCompetitionFutureModel
                    {
                        idCompetition = a.idCompetition,
                        name = a.Competition.name,
                        startDate = a.Competition.startDate,
                        endDate = a.Competition.endDate,
                        urlPosterPhoto = a.Competition.urlPosterPhoto
                    })
                    .OrderBy(a => a.endDate)
                    .ToList();

/*                var competitionsActive = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => currentDate < c.endDate && c.status == true && c.Assignements.Any(a => a.idUser == idJuror))
                .Select(c => new AllCompetitionFutureModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    urlPosterPhoto = c.urlPosterPhoto,
                })
                .OrderBy(c => c.endDate)
                .ToList();*/
                return competitionsActive;
            }
            else if(type == "vote progress")
            {
                var competitionsVote = assignementRepo.GetAssignementsIQueryable()
                .Where(a => a.idUser == idJuror && currentDate > a.Competition.endDate && currentDate < a.Competition.resultsDate && a.Competition.status == true)
                .Select(a => new AllCompetitionFutureModel
                {
                    idCompetition = a.idCompetition,
                    name = a.Competition.name,
                    startDate = a.Competition.startDate,
                    endDate = a.Competition.endDate,
                    urlPosterPhoto = a.Competition.urlPosterPhoto
                })
                .OrderBy(a => a.endDate)
                .ToList();

               /* var competitionsVote = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => currentDate > c.endDate && currentDate < c.resultsDate && c.status == true)
                .Select(c => new AllCompetitionFutureModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    urlPosterPhoto = c.urlPosterPhoto,
                })
                .OrderBy(c => c.endDate)
                .ToList();*/
                return competitionsVote;
            }
            else if(type == "done")
            {
                var competitionsDone = assignementRepo.GetAssignementsIQueryable()
                .Where(a => a.idUser == idJuror && currentDate >= a.Competition.resultsDate && a.Competition.status == true)
                .Select(a => new AllCompetitionFutureModel
                {
                    idCompetition = a.idCompetition,
                    name = a.Competition.name,
                    startDate = a.Competition.startDate,
                    endDate = a.Competition.endDate,
                    urlPosterPhoto = a.Competition.urlPosterPhoto
                })
                .OrderBy(a => a.endDate)
                .ToList();

               /* var competitionsDone = competitionRepo.GetCompetitionsIQueryable()
                .Where(c => currentDate >= c.resultsDate && c.status == true)
                .Select(c => new AllCompetitionFutureModel
                {
                    idCompetition = c.idCompetition,
                    name = c.name,
                    startDate = c.startDate,
                    endDate = c.endDate,
                    urlPosterPhoto = c.urlPosterPhoto,
                })
                .OrderBy(c => c.endDate)
                .ToList();*/
                return competitionsDone;
            }
            else
            {
                return new List<AllCompetitionFutureModel>();
            }
            
        }

        public List<CompetitionEmailPhotogrModel> GetPhEmail(int idComp) //get photographers email that has their photos reported
        {
            /*var participations = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => p.reportMessage == "DENIED" && p.idCompetition == idComp)
                .ToList();*/
            var participations = participationRepo.GetParticipationsSectionCompetition()
                                .Include(p => p.Photo)
                                .ThenInclude(ph => ph.Photographer)
                                .Include(p => p.Section)
                                .Where(p => p.reportMessage == "DENIED" && p.idCompetition == idComp)
                                .ToList();

            if (participations.Count == 0) return new List<CompetitionEmailPhotogrModel> { };

            var result = participations
                .GroupBy(p => new { p.Photo.Photographer.Email, p.Photo.Photographer.firstName, p.Photo.Photographer.lastName , p.Photo.Photographer.newsSubscription})
                .Select(g => new CompetitionEmailPhotogrModel
                {
                    emailPh = g.Key.Email,
                    namePh = g.Key.firstName + " " + g.Key.lastName,
                    newsSubscription = g.Key.newsSubscription,
                    photoDetails = g.Select(p => new PhotoDetailsReportedModel
                    {
                        photoUrl = p.Photo.photoUrl,
                        title = p.Photo.title,
                        sectionName = p.Section.name,
                    }).ToList()
                })
                .ToList();
            return result;
        }

        public List<CompetitionYearModel> GetYearsCompetitionsByPhotographer(string email)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idPhotographer == null) { return new List<CompetitionYearModel>(); }
            var competitions = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => p.Photo.Photographer.Id==idPhotographer && currentDate >= p.Competition.resultsDate )
                .Select(p => new CompetitionYearModel
                {
                    // year_ = p.Competition.resultsDate.Year.ToString()
                    year_ = p.Competition.startDate.Year.ToString()
                })
                .Distinct()
                .OrderByDescending(p => p.year_)
                .ToList();
           
            return competitions;
        }

        public List<NameCompJuror> GetNameCompJurors(string emailJ)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idJuror = users
                .Where(u => u.Email.Equals(emailJ))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idJuror == null) { return new List<NameCompJuror>(); }
            var competitions = assignementRepo.GetAssignementsIQueryable()
                .Where(a => a.idUser == idJuror && currentDate >= a.Competition.resultsDate)
                .Select(a => new NameCompJuror
                {
                    competitionName = a.Competition.name
                })
                .Distinct()
                .ToList();
            return competitions;
        }
    }
}
