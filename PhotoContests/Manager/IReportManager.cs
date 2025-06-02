using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IReportManager
    {
        void Create(ReportCreateModel model);
        void ReportResult(int idCompetition);
        ReportMessage GetReportMessage(int idCompetition, int idSection, int idPhoto, string emailUser);
        ReportsJurorModel GetAllParticipationPhReports(int idCompetition, int idSection, int idPhoto);
        NoDeclineAccModel GetAcceptDeclineReportJuror(int idCompetition, int idSection, int idPhoto, string emailJuror);
        bool GetExistParticipationReportsJuror(int idCompetition, int idSection, int idPhoto);
    }
}
