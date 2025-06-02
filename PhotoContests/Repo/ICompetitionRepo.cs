using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface ICompetitionRepo
    {
        void Create(Competition competition);
        void Update(Competition competition);
        void Delete(Competition competition);
        IQueryable<Competition> GetCompetitionsIQueryable();
    }
}
