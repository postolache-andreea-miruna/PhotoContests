using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface ICompetitionManager
    {
        void Create(CompetitionCreateModel model);
        void Update(CompetitionUpdateModel model);
        List<CompetitionGetAllModel?> GetAllCompetitions();
        List<CompetitionGetAllModel?> GetAllCompetitionsCurrentYear();
        List<CompetitionGetAllModel?> GetAllCompetitionsByYear(int year);



       // List<CompetitionGetAllModel?> GetAllActiveCompetitions();
        List<CompetitionGetAllParticpModel> GetAllActiveCompetitions();


        List<CompetitionByIdModel> GetCompetitionById(int id);
        List<CompetitionGetAllModel?> GetAllCompetitionsByPhotographer(string email);

        //new
        List<AllCompetitionActiveModel> GetAllCompetitionsActive(string email);
        List<AllCompetitionCompleteModel> GetAllCompetitionsCompleted(string email);
        List<AllCompetitionCompleteModel> GetAllCompetitionsFinished();
        List<AllCompetitionFutureModel> GetAllCompetitionsFuture();
        List<AllCompetitionFutureModel> GetAllCompetitionsJuror(string emailJuror, string type = "active");
        List<CompetitionGetAllInfoModel?> GetAllInfoCompetitions();

        List<PhotogrCompetitionsInJudgingModel> GetAllUserCompetitionsInJudging(string email);

        List<UserCompletedCompetitionsUsedModel> GetUserAllCompletedCompetitions(string email);
        List<UserAllActiveCompModel> GetUserAllActiveCompetitionsPhotos(string email);
        List<CompetitionGetAllParticpModel> GetAllActiveCurrentCompetitions();

        List<AllCompetitionsActiveJurorModel> GetAllActiveCompetitionsJuror(string emailJuror);
        List<AllCompetitionsActiveJurorModel> GetAllEndedCompetitionsJuror(string emailJuror);
        List<AllCompetitionsActiveJurorModel> GetAllFinishedCompetitionsJuror(string emailJuror);

        List<CompetitionNameModel> GetAllCompetitionsNamesByPhotographer(string email);

        List<CompetitionEmailPhotogrModel> GetPhEmail(int idComp);
        List<CompetitionYearModel> GetYearsCompetitionsByPhotographer(string email);
        List<NameCompJuror> GetNameCompJurors(string emailJ);
    }
}
