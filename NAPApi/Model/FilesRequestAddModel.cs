
namespace NAPApi.Model
{
    public class FilesRequestAddModel
    {
        public string FileName { get; set; }
        public IFormFile File { set; get; }
        public int IdGroup { set; get; }

    }
}
