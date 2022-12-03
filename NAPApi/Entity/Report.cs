using System.ComponentModel.DataAnnotations;

namespace NAPApi.Entity
{
    public class Report
    {
        public int ReportId { get; set; }
        public DateTime ReportLockDate { get; set; }
        public DateTime ReportUnLockDate { get; set; }
        public DateTime ReportUpdateDate { get; set; }

        //navigator
        public int FileId { set; get; }
        public Files File { get; set; }

        public int UserId { set; get; }
        public User User { get; set; }
    }
}
