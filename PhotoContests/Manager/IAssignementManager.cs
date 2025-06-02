using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IAssignementManager
    {
        void Create(AssignementCreateModel model);
        void Delete(string emailJuror, int idCompetition);
    }
}
