using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class NationalityRepo: INationalityRepo
    {
        private PhotoContestsContext db;
        public NationalityRepo(PhotoContestsContext db)
        {
            this.db = db;
        }
        public void Create(Nationality nationality)
        {
            db.Nationalities.Add(nationality);
            db.SaveChanges();
        }
        public void Update(Nationality nationality)
        {
            db.Nationalities.Update(nationality);
            db.SaveChanges();
        }
        public void Delete(Nationality nationality)
        {
            db.Nationalities.Remove(nationality);
            db.SaveChanges();
        }
        public IQueryable<Nationality> GetNationalitiesIQueryable()
        {
            var nationalities = db.Nationalities;
            return nationalities;
        }
    }
}
