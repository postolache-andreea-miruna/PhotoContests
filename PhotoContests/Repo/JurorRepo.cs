using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class JurorRepo: IJurorRepo
    {
        private PhotoContestsContext db;
        public JurorRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public IQueryable<Juror> GetJurorsIQueryable()
        {
            var jurors = db.Jurors;
            return jurors;
        }

        public void Update(Juror juror)
        {
            db.Jurors.Update(juror);
            db.SaveChanges();
        }
    }
}
