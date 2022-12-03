namespace NAPApi.Entity
{
    public class PermessionsGroup
    {
        public int PermessionsGroupId { get; set; }
        public int PermessionsGroupSharedId { get; set; }
        

        //navigator
        public int GroupId { set; get; }
        public Group Group { set; get; }
    }
}
