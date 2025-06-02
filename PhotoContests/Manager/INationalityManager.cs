using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface INationalityManager
    {
        void Create(NationalityCreateModel model);
        void Update(NationalityUpdateModel model);
        void Delete(int id);
        List<NationalityCreateModel?> GetAllNationalities();
        List<NationalityUpdateModel> GetAllNationalitiesWithId();
        List<NationalityUpdateModel> GetNationalitiesForCompSection(int idCompetition, int idSection);
    }
}
