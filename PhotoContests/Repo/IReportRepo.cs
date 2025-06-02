using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IReportRepo
    {
        void Create(Report report);
        void Update(Report report);
        void Delete(Report report);
        void DeleteReports(List<Report> reports);
        IQueryable<Report> GetReportIQueryable();
    }
}
