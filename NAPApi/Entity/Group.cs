using System.ComponentModel.DataAnnotations;

namespace NAPApi.Entity
{
    public class Group
    {
        public int GroupId { get; set; }
        [MaxLength(30)]
        public string GroupName { get; set; }
        
        //navigator
        public int UserId { get; set; }
        public User User { get; set; }

        public List<PermessionsGroup> permessionsGroups { get; set; }
        public List<Files> files { get; set; }
    }
}
