
namespace NAPApi.Help
{
    public class FileLogger
    {
        private static FileLogger _fileLogger;
        
        private FileLogger(){}

        public static FileLogger GetInstance()
        {
            if(_fileLogger == null)
            {
                _fileLogger = new FileLogger();
            }
            return _fileLogger;
        }

        public async void saveData(string logger)
        {
            try
            {
                string path = Directory.GetCurrentDirectory() + "\\Uploads";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    return;
                }
                path += "\\logger.log";
                using (StreamWriter fileStream = new StreamWriter(path,true))
                {
                    fileStream.WriteLine(logger);
                }
            }
            catch(Exception ex)
            {
                saveData("Logger: " + ex.Message);
            }
            

            return ;

        }
    }
}
