using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class SectionRepo: ISectionRepo
    {
        private PhotoContestsContext db;
        public SectionRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void Create(Section section)
        {
            db.Sections.Add(section);
            db.SaveChanges();
        }

        public void Update(Section section)
        {
            db.Sections.Update(section);
            db.SaveChanges();
        }

        public void Delete(Section section)
        {
            db.Sections.Remove(section);
            db.SaveChanges();
        }

        public IQueryable<Section> GetSectionsIQueryable()
        {
            var sections = db.Sections;
            return sections;
        }
    }
}
