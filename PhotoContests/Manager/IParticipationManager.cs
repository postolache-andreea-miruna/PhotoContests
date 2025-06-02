using Microsoft.AspNetCore.Mvc;
using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IParticipationManager
    {
        int Create(ParticipationCreateModel model);
        void Delete(int idPhoto, int idSection, int idCompetition);
        void UpdateNew(Participation2UpdateModel model);
        void Update(ParticipationUpdateModel model);
        List<ParticipationCompSectPhotosModel> GetAllParticipationPhotosByCompSect(int idComp, int idSect);
        List<ParticipationPhotosUserComSectModel> GetAllParticipationPhotosByCompSectGrouped(int idComp, int idSect);
        List<ParticipationCompSectPhotosPhModel> GetAllParticipationForUserComp(string email, int idComp);
        List<HistoryParticipationsUser> GetParticipationHistoryUser(string email);
        List<BestOfModel> GetBestOfSectionByPh(string email);
        List<ParticipationPhotoRankModel> GetPhParticipationsCompNationSection(int idComp, int idSection, string nation = "all nations");
/*        List<ParticipationPhotoRankModel> GetPhParticipationsCompNationSection(int idComp, string section, string nation = "all nations");*/
        List<ParticipationPhotoRankModel> GetParticipationByComp(int idComp, string firstName, string lastName);

        //new
        List<CompetitionSectionsModel> GetAllActiveCompetitionsUser(string email);
        WinnerPhotoModel GetPhotoSectionWinnerPreviousComp(int idSection, int idCompetition);
        ParticipationPhotosUserComSectModel GetAllMyParticipationPhotosByCompSectUser(int idComp, int idSect, string email);

        List<ActiveSectionCompetitionModel> GetActiveSectionsCompetition(int idCompetition);


        List<ParticipationsForVotingModel> GetParticipationsForVoting(int idSection, int idCompetition);
        List<WinnerModel> GetWinners(int idCompetition, int idSection);

        List<WinnerModel> GetWinnersBeforeResults(int idCompetition, int idSection);
        List<ParticipationCompSectPhotosModel> GetAllUserParticipationPhotosByCompSect(int idComp, int idSect, string email);
        List<ParticipationActiveJurorModel> GetParticipationActiveJuror(int idCompetition, int idSection);

        List<HistoryParticipationsUserNewModel> GetParticipationHistoryUserGrouped(string email);

        double GetNoPodiumCompStatistic(string emailPh);
        List<CompetitionStatisticsPhModel> GetCompYearSectPhStatistic(string emailPh, string year_, string section);
        double GetDeniedCompJStatistic(string emailJur, string compName);
    }
}
