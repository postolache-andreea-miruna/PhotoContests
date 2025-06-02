using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class PhotographerRepo: IPhotographerRepo
    {
        private PhotoContestsContext db;
        public PhotographerRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public IQueryable<Photographer> GetPhotographersIQueryable()
        {
            var photographers = db.Photographers;
            return photographers;
        }

        public void Update(Photographer photographer)
        {
            db.Photographers.Update(photographer);
            db.SaveChanges();
        }
    }
}
