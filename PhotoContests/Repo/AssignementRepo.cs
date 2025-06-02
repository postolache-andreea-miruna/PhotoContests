using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class AssignementRepo:IAssignementRepo
    {
        private PhotoContestsContext db;
        public AssignementRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void Create(Assignement assignement)
        {
            db.Assignments.Add(assignement);
            db.SaveChanges();
        }

        public void Delete(Assignement assignement)
        {
            db.Assignments.Remove(assignement);
            db.SaveChanges();
        }

        public IQueryable<Assignement> GetAssignementsIQueryable()
        {
            var assignements = db.Assignments;
            return assignements;
        }
    }
}
