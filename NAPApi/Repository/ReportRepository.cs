using NAPApi.Context;
using NAPApi.Model;

namespace NAPApi.Repository
{
    public class ReportRepository:IReportRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public ReportRepository(ApplicationDbContext applicationDbContext) {
            this.applicationDbContext = applicationDbContext;
        }

        public List<ReportModel> getReportByFile(int FileID)
        {
            List<ReportModel> reports = new List<ReportModel>();
            var report = (from rp in applicationDbContext.reports
                          where rp.FileId == FileID
                          orderby rp.Date
                          select rp
                          ).ToList();
            foreach(var r in report)
            {
                reports.Add(new ReportModel{
                    Date= r.Date,
                    FileId= r.FileId,
                    State   =r.State,
                    UserId = r.UserId
                });
            }
            return reports;
        }

        public List<ReportModel> getReportByUser(string UserName)
        {
            List<ReportModel> reports = new List<ReportModel>();
            var report = (from rp in applicationDbContext.reports
                          join us in applicationDbContext.users
                          on rp.UserId equals us.Id
                          where us.Username == UserName
                          orderby rp.Date
                          select rp
                          ).Distinct().ToList();
            foreach (var r in report)
            {
                reports.Add(new ReportModel
                {
                    Date = r.Date,
                    FileId = r.FileId,
                    State = r.State,
                    UserId = r.UserId
                });
            }
            return reports;
        }
    }
}
