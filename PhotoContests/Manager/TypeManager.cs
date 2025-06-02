using PhotoContests.Models;
using PhotoContests.Repo;

namespace PhotoContests.Manager
{
    public class TypeManager: ITypeManager
    {
        private readonly ITypeRepo typeRepo;

        public TypeManager(ITypeRepo typeRepo)
        {
            this.typeRepo = typeRepo;
        }

        public void Create(TypeCreateModel typeCreateModel)
        {
            var newType = new Entities.Type
            {
                competitionType = typeCreateModel.competitionType
            };

            typeRepo.Create(newType);
        }

        public void Update(TypeUpdateModel typeUpdateModel)
        {
            var type = typeRepo.GetTypesIQueryable().FirstOrDefault(ty => ty.idType == typeUpdateModel.idType);
            if (type == null) return;
            type.competitionType = typeUpdateModel.competitionType;
            typeRepo.Update(type);
        }

        public void Delete(int idType)
        {

            var type = typeRepo.GetTypesIQueryable().FirstOrDefault(ty => ty.idType == idType);
            if(type== null) return;
            typeRepo.Delete(type);
        }

        public List<TypeCreateModel?> GetAllTypes()
        {
            var types = typeRepo.GetTypesIQueryable();
            if(types == null)
            {
                return new List<TypeCreateModel>();
            }

            var models = types.Select(ty => new TypeCreateModel { competitionType = ty.competitionType })
                              .OrderBy(ty => ty.competitionType)
                              .ToList();
            return models;
        }

        public List<TypeUpdateModel> GetAllTypesWithId()
        {
            var types = typeRepo.GetTypesIQueryable();
            if (types == null)
            {
                return new List<TypeUpdateModel>();
            };

            var models = types.Select(ty => new TypeUpdateModel 
                                {
                                    idType= ty.idType,
                                    competitionType = ty.competitionType
                                })
                              .OrderBy(ty => ty.competitionType)
                              .ToList();
            return models;
        }

    }
}
