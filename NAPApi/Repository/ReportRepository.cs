using NAPApi.Context;
using NAPApi.Model;

namespace NAPApi.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public ReportRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public List<ReportModel> getReportByFile(int FileID)
        {
            List<ReportModel> reports = new List<ReportModel>();
            var report = (from rp in applicationDbContext.reports
                          join us in applicationDbContext.users
                            on rp.UserId equals us.Id
                          join fl in applicationDbContext.files
                            on rp.FileId equals fl.FilesId
                          join gr in applicationDbContext.groups
                            on fl.GroupId equals gr.GroupId
                          where rp.FileId == FileID
                          orderby rp.Date
                          select new
                          {
                              Date = rp.Date,
                              State = rp.State,
                              NameFile = "Group: "+ gr.GroupName + " File: " + fl.FileName,
                              Name = us.Username
                          }
                          ).ToList();
            foreach (var r in report)
            {
                reports.Add(new ReportModel
                {
                    Date = r.Date,
                    NameFile = r.NameFile,
                    State = r.State,
                    Username = r.Name
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
                          join fl in applicationDbContext.files
                          on rp.FileId equals fl.FilesId
                          join gr in applicationDbContext.groups
                            on fl.GroupId equals gr.GroupId
                          where us.Username == UserName
                          orderby rp.Date
                          select new
                          {
                              Date = rp.Date,
                              NameFile= "Group: " + gr.GroupName + " File: " + fl.FileName,
                              State = rp.State,
                              Username = us.Username
                          }
                          ).Distinct().ToList();
            foreach (var r in report)
            {
                reports.Add(new ReportModel
                {
                    Date = r.Date,
                    NameFile = r.NameFile,
                    State = r.State,
                    Username = r.Username
                });
            }
            return reports;
        }
    }
}
