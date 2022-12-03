using System.ComponentModel.DataAnnotations;

namespace NAPApi.Entity
{
    public class Role
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }

        //navigator
        public List<User>users { get; set; }
    }
}
