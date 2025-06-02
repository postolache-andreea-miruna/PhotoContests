using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class TypeRepo: ITypeRepo
    {
        private PhotoContestsContext db;
        public TypeRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void Create(Entities.Type type)
        {
            db.Types.Add(type);
            db.SaveChanges();
        }

        public void Update(Entities.Type type)
        {
            db.Types.Update(type);
            db.SaveChanges();
        }

        public void Delete(Entities.Type type)
        {
            db.Types.Remove(type);
            db.SaveChanges();
        }

        public IQueryable<Entities.Type> GetTypesIQueryable()
        {
            var types = db.Types;
            return types;
        }
    }
}
