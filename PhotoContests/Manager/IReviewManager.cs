using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IReviewManager
    {
        void Create(ReviewCreateModel model);
        void Delete(int idReview);
        void Update(ReviewJurorUpdateModel model);
        List<ReviewModel> GetAllReviewsPhotoCompSect(int idPhoto, int idComp, int idSection);
        List<ReviewModel> GetAllReviewsForPhoto(int idPhoto);
        List<ReviewJurorGivenModel> GetReviewPartByJuror(string emailJuror, int idPhoto, int idCompetition, int idSection);
    }
}
