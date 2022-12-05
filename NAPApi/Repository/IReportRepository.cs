using NAPApi.Model;

namespace NAPApi.Repository
{
    public interface IReportRepository
    {
        List<ReportModel> getReportByUser(String UserName );
        List<ReportModel> getReportByFile(int FileID );
    }
}
