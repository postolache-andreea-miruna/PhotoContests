using Microsoft.AspNetCore.Identity;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;
using System;
using System.Runtime.CompilerServices;
using static System.Collections.Specialized.BitVector32;

namespace PhotoContests.Manager
{
    public class VideoManager:IVideoManager
    {
        private readonly IVideoRepo videoRepo;
        private readonly IParticipationRepo participationRepo;
        private readonly UserManager<User> userManager;
        public VideoManager(IVideoRepo videoRepo, UserManager<User> userManager, IParticipationRepo participationRepo)
        {
            this.videoRepo = videoRepo;
            this.userManager = userManager;
            this.participationRepo = participationRepo;
        }
        public void Delete(int idVideo)
        {
            var video = videoRepo.GetVideosIQueryable().FirstOrDefault(v=> v.idVideo == idVideo);
            if (video == null) return;
            videoRepo.Delete(video);
        }

        public void Create(VideoCreateModel model)
        {
            var users = userManager.Users;
            var jurorId = users.Where(u => u.Email.Equals(model.emailJuror)).Select(u => u.Id).FirstOrDefault();
            var newVideo = new Video
            {
                JurorId = jurorId,
                date = DateTime.Now,
                videoUrl = model.videoUrl,
                videoYTCode = model.videoYTCode,
                idPhoto= model.idPhoto,
                idCompetition= model.idCompetition,
                idSection= model.idSection
            };
            videoRepo.Create(newVideo);
        }
        public void Update(VideoUpdateModel model)
        {
           var video = videoRepo.GetVideosIQueryable().FirstOrDefault(v => v.idVideo == model.idVideo);
           video.videoUrl = model.videoUrl;
           video.videoYTCode = model.videoYTCode;
           videoRepo.Update(video);
        }
        public VideoPartJurorModel GetVideoForPartByJuror(string emailJuror, int idPhoto, int idCompetition, int idSection)
        {
            var users = userManager.Users;
            var jurorId = users.Where(u => u.Email.Equals(emailJuror)).Select(u => u.Id).FirstOrDefault();
            var video = videoRepo.GetVideosIQueryable()
                        .Where(v => v.JurorId == jurorId && v.idPhoto == idPhoto && v.idCompetition == idCompetition && v.idSection == idSection)
                        .Select(v => new VideoPartJurorModel
                        {
                            idVideo = v.idVideo,
                            idSection = v.idSection,
                            idCompetition = v.idCompetition,
                            idPhoto = v.idPhoto,
                            videoUrl = v.videoUrl,
                            videoYTCode = v.videoYTCode
                        })
                        .FirstOrDefault();
            if (video == null) return new VideoPartJurorModel();
            return video;
        }

/*        public List<VideosPhotogr> VideosReceivedByPhotogr(string emailPhotogr)
        {
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(emailPhotogr))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (idPhotographer == null) { return new List<VideosPhotogr>(); }

            var videos = videoRepo.GetVideosIQueryable()
                .Where(v => v.Participation.Photo.idUser == idPhotographer)
                .Select(v => new 
                {
                    idVideo = v.idVideo, 
                    date = v.date,
                    videoUrl = v.videoUrl,
                    videoYT = v.videoYTCode,
                    sectionName = v.Participation.Section.name,
                    competitionName = v.Participation.Competition.name,
                    idCompetition = v.Participation.Competition.idCompetition
                })
                .ToList();

            var groupedByCompetition = videos
                .GroupBy(v => new { v.idCompetition, v.competitionName })
                .Select(compGroup => new VideosPhotogr
                {
                    idCompetition = compGroup.Key.idCompetition,
                    competitionName = compGroup.Key.competitionName,
                    sectionVideosDetails = compGroup
                        .GroupBy(v => new { v.sectionName })
                        .Select(secGroup => new SectionsVideoModel
                        {
                            sectionName = secGroup.Key.sectionName,
                            videosDetails = secGroup.Select(v => new VideosDetailsModel
                            {
                                idVideo = v.idVideo,
                                date = v.date,
                                videoUrl = v.videoUrl,
                                videoYTCode = v.videoYT
                            }).ToList()
                        }).ToList()
                })
                .ToList();

            return groupedByCompetition;
        }*/
        public List<VideosPhotogr> VideosReceivedByPhotogr(string emailPhotogr, string competitionName)
        {
            var groupedByCompetition = new List<VideosPhotogr>();
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(emailPhotogr))
                .Select(u => u.Id)
                .FirstOrDefault();
            if (idPhotographer == null) { return new List<VideosPhotogr>(); }

            if (competitionName == "all")
            {
                var currentDate = DateTime.Now;
                var videos = videoRepo.GetVideosIQueryable()
                .Where(v => v.Participation.Photo.idUser == idPhotographer
                 && v.Participation.Competition.resultsDate <= currentDate)
                .Select(v => new
                {
                    idVideo = v.idVideo,
                    date = v.date,
                    videoUrl = v.videoUrl,
                    videoYT = v.videoYTCode,
                    sectionName = v.Participation.Section.name,
                    competitionName = v.Participation.Competition.name,
                    idCompetition = v.Participation.Competition.idCompetition
                })
                .ToList();

                groupedByCompetition = videos
                    .GroupBy(v => new { v.idCompetition, v.competitionName })
                    .Select(compGroup => new VideosPhotogr
                    {
                        idCompetition = compGroup.Key.idCompetition,
                        competitionName = compGroup.Key.competitionName,
                        sectionVideosDetails = compGroup
                            .GroupBy(v => new { v.sectionName })
                            .Select(secGroup => new SectionsVideoModel
                            {
                                sectionName = secGroup.Key.sectionName,
                                videosDetails = secGroup.Select(v => new VideosDetailsModel
                                {
                                    idVideo = v.idVideo,
                                    date = v.date,
                                    videoUrl = v.videoUrl,
                                    videoYTCode = v.videoYT
                                }).ToList()
                            }).ToList()
                    })
                    .ToList();
            }
            else
            {
                var videos = videoRepo.GetVideosIQueryable()
                .Where(v => v.Participation.Photo.idUser == idPhotographer && v.Participation.Competition.name == competitionName)
                .Select(v => new
                {
                    idVideo = v.idVideo,
                    date = v.date,
                    videoUrl = v.videoUrl,
                    videoYT = v.videoYTCode,
                    sectionName = v.Participation.Section.name,
                    competitionName = v.Participation.Competition.name,
                    idCompetition = v.Participation.Competition.idCompetition
                })
                .ToList();
                groupedByCompetition = videos
                    .GroupBy(v => new { v.idCompetition, v.competitionName })
                    .Select(compGroup => new VideosPhotogr
                    {
                        idCompetition = compGroup.Key.idCompetition,
                        competitionName = compGroup.Key.competitionName,
                        sectionVideosDetails = compGroup
                            .GroupBy(v => new { v.sectionName })
                            .Select(secGroup => new SectionsVideoModel
                            {
                                sectionName = secGroup.Key.sectionName,
                                videosDetails = secGroup.Select(v => new VideosDetailsModel
                                {
                                    idVideo = v.idVideo,
                                    date = v.date,
                                    videoUrl = v.videoUrl,
                                    videoYTCode = v.videoYT
                                }).ToList()
                            }).ToList()
                    })
                    .ToList();
            }

            return groupedByCompetition;
        }
        /*public void Delete(string idPhotographer, string idJuror, DateTime date)
        {
            var video = videoRepo.GetVideosIQueryable()
                .FirstOrDefault(v => v.idPhotographer == idPhotographer && v.idJuror == idJuror && v.date == date);
            if (video == null) return;
            videoRepo.Delete(video);
        }
        public void Create(VideoCreateModel model)
        {
            var users = userManager.Users;
            var photographerId = users.Where(u => u.Email.Equals(model.emailPhotographer)).Select(u => u.Id).FirstOrDefault();
            var jurorId = users.Where(u => u.Email.Equals(model.emailJuror)).Select(u => u.Id).FirstOrDefault();

            var newVideo = new Video
            {
                idPhotographer = photographerId,
                idJuror = jurorId,
                date = DateTime.Now,
                videoUrl = model.videoUrl,
                videoYTCode = model.videoYTCode,
                name = model.name
            };
            videoRepo.Create(newVideo);
        }

        public List<VideoModel> GetAllVideosByPhotogrAndName(string email, string name = "all competitions")
        {
            var users = userManager.Users;
            var photographerId = users.Where(u => u.Email.Equals(email)).Select(u => u.Id).FirstOrDefault();
            if (name == "all competitions")
            {
                var videos = videoRepo.GetVideosIQueryable()
                .Where(v => v.idPhotographer == photographerId)
                .Select(v => new VideoModel
                {
                    date = v.date,
                    videoUrl = v.videoUrl,
                    videoYTCode = v.videoYTCode,
                    name = v.name
                })
                .OrderByDescending(v => v.date)
                .ThenBy(v => v.name)
                .ToList();
                return videos;
            }
            else
            {
                var videos = videoRepo.GetVideosIQueryable()
                .Where(v => v.idPhotographer == photographerId && v.name == name)
                .Select(v => new VideoModel
                {
                    date = v.date,
                    videoUrl = v.videoUrl,
                    videoYTCode = v.videoYTCode,
                    name = v.name
                })
                .OrderByDescending(v => v.date)
                .ToList();
                return videos;
            }
        }

        public List<VideoJurorMode> GetAllVideosByJurorAndName(string emailJuror, string name = "all competitions")
        {
            var users = userManager.Users;
            var jurorId = users.Where(u => u.Email.Equals(emailJuror)).Select(u => u.Id).FirstOrDefault();
            if (name == "all competitions")
            {
                var videos = videoRepo.GetVideosIQueryable()
                .Where(v => v.idJuror == jurorId)
                .Select(v => new VideoJurorMode
                {
                    usernamePh = users.Where(u => u.Id.Equals(v.idPhotographer)).Select(u => u.firstName) + " " + users.Where(u => u.Id.Equals(v.idPhotographer)).Select(u => u.lastName),
                    date = v.date,
                    videoUrl = v.videoUrl,
                    videoYTCode = v.videoYTCode,
                    name = v.name
                })
                .OrderByDescending(v => v.date)
                .ThenBy(v => v.name)
                .ToList();
                return videos;
            }
            else
            {
                var videos = videoRepo.GetVideosIQueryable()
                .Where(v => v.idJuror == jurorId && v.name == name)
                .Select(v => new VideoJurorMode
                {
                    usernamePh = users.Where(u => u.Id.Equals(v.idPhotographer)).Select(u => u.firstName) + " " + users.Where(u => u.Id.Equals(v.idPhotographer)).Select(u => u.lastName),
                    date = v.date,
                    videoUrl = v.videoUrl,
                    videoYTCode = v.videoYTCode,
                    name = v.name
                })
                .OrderByDescending(v => v.date)
                .ToList();
                return videos;
            }
        }*/

    }
}
