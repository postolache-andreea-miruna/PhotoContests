namespace PhotoContests.Repo
{
    public interface ITypeRepo
    {
        void Create(Entities.Type type);
        void Update(Entities.Type type);
        void Delete(Entities.Type type);
        IQueryable<Entities.Type> GetTypesIQueryable();
    }
}
