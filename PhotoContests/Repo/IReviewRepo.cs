using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IReviewRepo
    {
        void Create(Review review);
        void Update(Review review);
        void Delete(Review review);
        IQueryable<Review> GetReviewsIQueryable();
    }
}
