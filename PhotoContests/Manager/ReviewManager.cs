using Microsoft.AspNetCore.Identity;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;

namespace PhotoContests.Manager
{
    public class ReviewManager:IReviewManager
    {
        private readonly IReviewRepo reviewRepo;
        private readonly ICompetitionRepo competitionRepo;
        private readonly ISectionRepo sectionRepo;
        private readonly UserManager<User> userManager;
        public ReviewManager(IReviewRepo reviewRepo, ICompetitionRepo competitionRepo, ISectionRepo sectionRepo, UserManager<User> userManager)
        {
            this.reviewRepo = reviewRepo;
            this.competitionRepo = competitionRepo;
            this.sectionRepo = sectionRepo;
            this.userManager = userManager;
        }

        public void Create(ReviewCreateModel model)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(model.emailUser)).Select(u => u.Id).FirstOrDefault();

            var user = users.Where(u => u.Email.Equals(model.emailUser)).FirstOrDefault();
            var newReview = new Review
            {
                idUser = userId,
                idPhoto = model.idPhoto,
                idCompetition = model.idCompetition,
                idSection = model.idSection,
                message = model.message,
                sendDate = DateTime.Now
            };

            reviewRepo.Create(newReview);
        }

        public void Delete(int idReview)
        {
            var review = reviewRepo.GetReviewsIQueryable()
                .FirstOrDefault(r => r.idReview == idReview);
            if (review == null) return;
            reviewRepo.Delete(review);
        }

        public void Update(ReviewJurorUpdateModel model)
        {
            var review = reviewRepo.GetReviewsIQueryable().FirstOrDefault(r => r.idReview.Equals(model.idReview));
            review.message = model.message;
            reviewRepo.Update(review);
        }

        public List<ReviewModel> GetAllReviewsPhotoCompSect(int idPhoto, int idComp, int idSection)
        {
            var competition = competitionRepo.GetCompetitionsIQueryable().FirstOrDefault(c => c.idCompetition== idComp);
            var section = sectionRepo.GetSectionsIQueryable().FirstOrDefault(s => s.idSection== idSection);
            var review = reviewRepo.GetReviewsIQueryable()
                .Where(r => r.idPhoto == idPhoto && r.idCompetition == idComp && r.idSection == idSection)
                .Select(r => new ReviewModel
                {
                    competition = competition.name,
                    section = section.name,
                    message = r.message,
                    sendDate = r.sendDate
                })
                .OrderByDescending(r => r.sendDate)
                .ToList();
            return review;
        }

        public List<ReviewModel> GetAllReviewsForPhoto(int idPhoto)
        {
           
            var review = reviewRepo.GetReviewsIQueryable()
                .Where(r => r.idPhoto == idPhoto)
                .Select(r => new ReviewModel
                {
                    competition = competitionRepo.GetCompetitionsIQueryable().Where(c => c.idCompetition == r.idCompetition).Select(c => c.name).FirstOrDefault(),
                    section = sectionRepo.GetSectionsIQueryable().Where(s => s.idSection == r.idSection).Select(s => s.name).FirstOrDefault(),
                    message = r.message,
                    sendDate = r.sendDate
                })
                .OrderByDescending(r => r.sendDate)
                .ThenBy(r => r.competition)
                .ThenBy(r => r.section)
                .ToList();
            return review;
        }

        public List<ReviewJurorGivenModel> GetReviewPartByJuror(string emailJuror, int idPhoto, int idCompetition, int idSection)
        {
            var users = userManager.Users;
            var userId = users.Where(u => u.Email.Equals(emailJuror)).Select(u => u.Id).FirstOrDefault();
            var user = users.Where(u => u.Email.Equals(emailJuror)).FirstOrDefault();

            var review = reviewRepo.GetReviewsIQueryable()
                .Where(r => r.idPhoto == idPhoto && r.idUser == userId && r.idCompetition == idCompetition && r.idSection == idSection)
                .Select(r => new ReviewJurorGivenModel
                {
                    idReview = r.idReview,
                    idPhoto = idPhoto,
                    idCompetition = idCompetition,
                    idSection = idSection,
                    idUser = r.idUser,
                    message= r.message,
                    sendDate=r.sendDate,
                    emailJuror = users.Where(u => u.Id.Equals(r.idUser)).Select(u => u.Email).FirstOrDefault()
                })
                .OrderByDescending(r => r.sendDate)
                .ToList();
            if(review == null || user is not Juror)
            {
                return new List<ReviewJurorGivenModel>();
            }
            return review;
            
        }

    }
}
