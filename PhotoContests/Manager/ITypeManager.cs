using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface ITypeManager
    {
        void Create(TypeCreateModel typeCreateModel);
        void Update(TypeUpdateModel typeUpdateModel);
        void Delete(int idType);
        List<TypeCreateModel?> GetAllTypes();
        List<TypeUpdateModel> GetAllTypesWithId();
    }
}
