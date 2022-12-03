using System.ComponentModel.DataAnnotations;

namespace NAPApi.Entity
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string Username { get; set; }
        [MaxLength(60)]
        public string Password { get; set; }
        [MaxLength(40)]
        public string Email { get; set; }
        public bool Confirm { get; set; }

        // navigation
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public List<Logging> Loggings { set; get; }
        public List<Report> Reports { set; get; }
        public List<Group> groups { set; get; }
    }
}
