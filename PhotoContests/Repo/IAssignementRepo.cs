using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IAssignementRepo
    {
        void Create(Assignement assignement);
        void Delete(Assignement assignement);
        IQueryable<Assignement> GetAssignementsIQueryable();
    }
}
