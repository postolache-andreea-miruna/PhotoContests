using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IJurorRepo
    {
        IQueryable<Juror> GetJurorsIQueryable();
        void Update(Juror juror);
    }
}
