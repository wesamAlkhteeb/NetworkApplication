using System.ComponentModel.DataAnnotations;

namespace NAPApi.Entity
{
    public class Logging
    {
        public int LoggingId { get; set; }
        [MaxLength(100)]
        public string LoggingAction { get; set; }

        //navigator
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
