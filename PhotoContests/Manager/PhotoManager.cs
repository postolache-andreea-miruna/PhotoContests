using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using PhotoContests.Entities;
using PhotoContests.Migrations;
using PhotoContests.Models;
using PhotoContests.Repo;
using System.Globalization;
using System.Net.WebSockets;
using System.Reflection;
using static System.Collections.Specialized.BitVector32;

namespace PhotoContests.Manager
{
    public class PhotoManager:IPhotoManager
    {
        private readonly IPhotoRepo photoRepo;
        private readonly UserManager<User> userManager;
        private readonly IParticipationRepo participationRepo;
        public PhotoManager(IPhotoRepo photoRepo, UserManager<User> userManager, IParticipationRepo participationRepo)
        {
            this.photoRepo = photoRepo;
            this.userManager = userManager;
            this.participationRepo = participationRepo;
        }
        ///newwwwwwwwwwwwww
/*        public int GetExifValue(System.Drawing.Image image, int tag)
        {
            var propertyItem = image.GetPropertyItem(tag);
            return BitConverter.ToInt16(propertyItem.Value, 0); 
        }
        public float GetExifRationalValue(System.Drawing.Image image, int tag)
        {
            var propertyItem = image.GetPropertyItem(tag);
            int numerator = BitConverter.ToInt32(propertyItem.Value, 0);
            int denominator = BitConverter.ToInt32(propertyItem.Value, 4);
            return (float)numerator / denominator;
        }
        public DateTime GetExifDate(System.Drawing.Image image, int tag)
        {
            var propertyItem = image.GetPropertyItem(tag);
            string dateString = System.Text.Encoding.ASCII.GetString(propertyItem.Value);
            return DateTime.ParseExact(dateString, "yyyy:MM:dd HH:mm:ss", null); 
        }
        public string GetExifCameraModel(System.Drawing.Image image, int tag)
        {
            var propertyItem = image.GetPropertyItem(tag);
            string cameraModel = System.Text.Encoding.ASCII.GetString(propertyItem.Value).Trim();
            return cameraModel;
        }*/



        public int Create(PhotoCreateModel model)
        {
            var users = userManager.Users;
            var idPhotographer = users.Where(u => u.Email.Equals(model.emailUser))
                .Select(u => u.Id).FirstOrDefault();
            ////////////////////////////////////////new
/*            var stream = model.PhotoRawData.OpenReadStream();
            var photo = System.Drawing.Image.FromStream(stream);
            var iso = GetExifValue(photo, 34855);
            var fNumber = GetExifRationalValue(photo, 33437);
            var dataTaken = GetExifDate(photo, 306);
            var cameraModel = GetExifCameraModel(photo, 0x0110);
            var exposure = GetExifRationalValue(photo, 0x829A);*/
            ////////////////////new
                
            var newPhoto = new Photo
            {
                downloadTime = DateTime.Now,
                title = model.title,
                description = model.description,
                photoUrl = model.photoUrl,
                visibility = model.visibility,
                idUser = idPhotographer,
                takenDate = model.takenDate,
                cameraModel = model.cameraModel,
                fStop = model.fStop,
                exposureTime = model.exposureTime,
                isoSpeed = model.isoSpeed
            };
            photoRepo.Create(newPhoto);
            return newPhoto.idPhoto;
        }

        public void Update(PhotoUpdateModel model)
        {
            var photo = photoRepo.GetPhotosIQueryable()
                .FirstOrDefault(p => p.idPhoto == model.idPhoto);
            if (photo == null) return;
            photo.title = model.title;
            photo.description = model.description;
            photo.visibility = model.visibility;
            
            photoRepo.Update(photo);
        }

        public void Delete(int id)
        {
            var photo = photoRepo.GetPhotosIQueryable()
                .FirstOrDefault(p => p.idPhoto == id);
            if(photo== null) return;    
            photoRepo.Delete(photo);
        }


        //all photos by emailUser
        public List<GetAllPhotosModel> GetAllPhotosByPhotographer(string email)
        {
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            var photos = photoRepo.GetPhotosIQueryable().Where(p => p.idUser == idPhotographer)
                .Select(p => new GetAllPhotosModel
                {
                    idPhoto = p.idPhoto,
                    photoUrl= p.photoUrl,
                    downloadTime= p.downloadTime
                })
                .OrderByDescending(p => p.downloadTime)
                .ToList();
            return photos;
        }

        public List<GetAllPhotosModel> GetAllPhotosDontPartByPhotographer(string email, int idComp, int idSect)
        {
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();

            var participations = participationRepo.GetParticipationsSectionCompetitionPhoto()
                                .Where(part => part.idCompetition == idComp && part.idSection == idSect)
                                .Select(part => part.idPhoto)
                                .ToHashSet();

            var photos = photoRepo.GetPhotosIQueryable()
                .Where(p => p.idUser == idPhotographer && !participations.Contains(p.idPhoto))
                .Select(p => new GetAllPhotosModel
                {
                    idPhoto = p.idPhoto,
                    photoUrl = p.photoUrl,
                    downloadTime = p.downloadTime
                })
                .OrderByDescending(p => p.downloadTime)
                .ToList();
            return photos;
        }

        //photo portofolio for a given user
        public List<PortofolioPhotosModel> GetPortofolioPhotographer(string email)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            var photos = photoRepo.GetPhotosIQueryable()
                            .Where(ph => ph.idUser == idPhotographer)
                            .Select(ph => new PortofolioPhotosModel
                            {
                                idPhoto = ph.idPhoto,
                                photoUrl = ph.photoUrl,
                                downloadTime = ph.downloadTime,
                                visibility = ph.visibility,
                                description = ph.description,
                                title = ph.title,
                                reviews = ph.Participations
                                .Where(p => currentDate >= p.Competition.resultsDate)//new
                                .SelectMany(p => p.Reviews)
                                .Select(r => new ReviewModel
                                {
                                    competition = r.Participation.Competition.name,
                                    section = r.Participation.Section.name,
                                    message = r.message,
                                    sendDate = r.sendDate
                                })
                                .OrderByDescending(r => r.sendDate)
                                .ToList(),
                                results = ph.Participations
                                    .Where(p => currentDate>= p.Competition.resultsDate)
                                    .Select(p => new ParticipationModel
                                    {
                                        competition = p.Competition.name,
                                        idCompetition = p.Competition.idCompetition,
                                        competitionCover = p.Competition.urlPosterPhoto,
                                        endDateComp = p.Competition.endDate,
                                        section = p.Section.name,         
                                        ranking = p.ranking,                 
                                        totalPoints = p.totalPoints,
                                        finalResult = p.finalResult
                                    })
                                    .OrderByDescending(p => p.endDateComp) 
                                    .ToList()
                            })
                            .ToList();
            return photos;
        }

        //portofolio viewed by others
        public List<PortofolioPhotosOtherViewModel> GetPortofolioPhotographerOtherView(string email)
        {
            //
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            var photos = photoRepo.GetPhotosIQueryable()
                            .Where(ph => ph.idUser == idPhotographer && ph.visibility == true)
                            .Select(ph => new PortofolioPhotosOtherViewModel
                            {
                                idPhoto = ph.idPhoto,
                                photoUrl = ph.photoUrl,
                                downloadTime = ph.downloadTime,
                                
                                description = ph.description,
                                title = ph.title,
                                
                                results = ph.Participations
                                .Where(p => currentDate>= p.Competition.resultsDate )//new 14
                                    .Select(p => new ParticipationModel
                                    {
                                        competition = p.Competition.name,
                                        idCompetition = p.Competition.idCompetition,
                                        competitionCover = p.Competition.urlPosterPhoto,
                                        endDateComp = p.Competition.endDate,
                                        section = p.Section.name,
                                        ranking = p.ranking,
                                        totalPoints = p.totalPoints,
                                        finalResult = p.finalResult
                                    })
                                    .OrderByDescending(p => p.endDateComp)
                                    .ToList()
                            })
                            .ToList();
            return photos;
        }

        //all photos by emailUser that are visible
        public List<GetAllPhotosModel> GetAllPhotosByPhotographerVisible(string email)
        {
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();
            var photos = photoRepo.GetPhotosIQueryable().Where(p => p.idUser == idPhotographer && p.visibility == true)
                .Select(p => new GetAllPhotosModel
                {
                    idPhoto = p.idPhoto,
                    photoUrl = p.photoUrl,
                    downloadTime= p.downloadTime
                })
                .OrderByDescending (p => p.downloadTime)
                .ToList();
            return photos;
        }

        //photo by id

        public GetPhotoModel GetPhotoInfo(int id)
        {
            var photo = photoRepo.GetPhotosIQueryable().Where(p => p.idPhoto == id)
                .Select(p => new GetPhotoModel
                {
                    idPhoto = p.idPhoto,
                    title = p.title,
                    description = p.description,
                    photoUrl = p.photoUrl,
                    visibility = p.visibility,
                    namePhotographer = p.Photographer.firstName +" "+p.Photographer.lastName,
                    downloadTime= p.downloadTime,

                    takenDate = p.takenDate,
                    cameraModel = p.cameraModel,
                    fStop = p.fStop,
                    exposureTime = p.exposureTime,
                    isoSpeed = p.isoSpeed,
                    emailPhotographer = p.Photographer.Email,
                    newsSubscription = p.Photographer.newsSubscription,
                    
                })
                .FirstOrDefault();
            return photo;
        }


    }
}
