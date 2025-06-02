using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class ReportRepo: IReportRepo
    {
        private PhotoContestsContext db;
        public ReportRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void Create(Report report)
        {
            db.Reports.Add(report);
            db.SaveChanges();
        }

        public void Update(Report report)
        {
            db.Reports.Update(report);
            db.SaveChanges();
        }

        public void Delete(Report report)
        {
            db.Reports.Remove(report);
            db.SaveChanges();
        }

        public void DeleteReports(List<Report> reports)
        {
            db.Reports.RemoveRange(reports);
            db.SaveChanges();
        }
        public IQueryable<Report> GetReportIQueryable()
        {
            var reports = db.Reports;
            return reports;
        }
    }
}
