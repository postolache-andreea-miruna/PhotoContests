using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class CompetitionRepo:ICompetitionRepo
    {
        private PhotoContestsContext db;

        public CompetitionRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void Create(Competition competition)
        {
            db.Competitions.Add(competition);
            db.SaveChanges();
        }

        public void Update(Competition competition)
        {
            db.Competitions.Update(competition);
            db.SaveChanges();
        }

        public void Delete(Competition competition)
        {
            db.Competitions.Remove(competition);
            db.SaveChanges();
        }

        public IQueryable<Competition> GetCompetitionsIQueryable()
        {
            var competitions = db.Competitions;
            return competitions;
        }
        
    }
}
