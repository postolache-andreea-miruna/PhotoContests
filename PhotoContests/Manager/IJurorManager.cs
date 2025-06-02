using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IJurorManager
    {
        JurorByEmailModel GetJurorInfo(string email);
        JurorByIdModel GetJurorInfoById(string id);
        List<JurorByIdModel> GetAllJurorsByCompetitionId(int competitionId);
        List<JurorGetAllModel> GetAllJurors();
        List<JurorByIdModel> GetAllJurorsNotInCompById(int competitionId);

        JurorByEmailModel GetJurorInfoByID(string id);
        void Update(JurorUpdateModel model);
    }
}
