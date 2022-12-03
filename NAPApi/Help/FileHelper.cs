using System.Diagnostics;
namespace NAPApi.Help
{
    public class FileHelper
    {
        
        public async Task<string> SaveFile(IFormFile file , string pathFile)
        {
            if(file.Length > 0)
            {
                try
                {
                    string pathFolder = Directory.GetCurrentDirectory() + "\\Uploads";
                    if (!Directory.Exists(pathFolder))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }
                    string path = pathFolder + pathFile;
                    Debug.WriteLine(path);
                    using (FileStream fileStream  = new FileStream(path,FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        fileStream.Flush();
                    }
                    return "\\Uploads"+pathFile;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        public bool DeleteImage(string path)
        {
            
            path = Directory.GetCurrentDirectory()  + path.Replace("/","\\");
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }else
            {
                return false;
            }
        } 

        public async Task<bool> UpdateFile(string path , IFormFile file )
        {
            if (!DeleteImage(path))
            {
                return false;
            }
            string name = "\\"+ path.Split("\\").Last();
            await SaveFile(file, name);

            return true;
        }


        public string GenerateName(IFormFile file)
        {
            string[] np = file.FileName.Split(".");
            return "\\" + getNameFile(np[0]) + DateTime.Now.GetHashCode() + new Random().Next(100) + "." + np[1];
        }

        private string getNameFile(string namefile)
        {
            string name = string.Empty;
            string[] a = namefile.Split(" ");
            foreach (var b in a)
            {
                name += b;
            }
            return name;
        }
    }
}
