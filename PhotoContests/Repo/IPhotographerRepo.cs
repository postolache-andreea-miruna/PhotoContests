using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IPhotographerRepo
    {
        IQueryable<Photographer> GetPhotographersIQueryable();
        void Update(Photographer photographer);
    }
}
