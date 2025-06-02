using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface ISectionRepo
    {
        void Create(Section section);
        void Update(Section section);
        void Delete(Section section);
        IQueryable<Section> GetSectionsIQueryable();
    }
}
