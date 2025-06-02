using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class ReviewRepo:IReviewRepo
    {
        private PhotoContestsContext db;
        public ReviewRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void Create(Review review)
        {
            db.Reviews.Add(review);
            db.SaveChanges();
        }
        public void Update(Review review)
        {
            db.Reviews.Update(review);
            db.SaveChanges();
        }
        public void Delete(Review review)
        {
            db.Reviews.Remove(review);
            db.SaveChanges();
        }

        public IQueryable<Review> GetReviewsIQueryable()
        {
            var reviews = db.Reviews;
            return reviews;
        }
    }
}
