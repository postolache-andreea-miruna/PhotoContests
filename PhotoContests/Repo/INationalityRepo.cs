using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface INationalityRepo
    {
        void Create(Nationality nationality);
        void Update(Nationality nationality);
        void Delete(Nationality nationality);
        IQueryable<Nationality> GetNationalitiesIQueryable();
    }
}
