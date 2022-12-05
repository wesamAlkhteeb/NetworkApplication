using System.ComponentModel.DataAnnotations;

namespace NAPApi.Entity
{
    public class Report
    {
        public int ReportId { get; set; }
        [Required , MaxLength(6)]
        public string State { get; set; }
        public DateTime Date { get; set; }
        
        //navigator
        public int FileId { set; get; }
        public Files File { get; set; }

        public int UserId { set; get; }
        public User User { get; set; }
    }
}
