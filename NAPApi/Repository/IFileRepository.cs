using NAPApi.Model;

namespace NAPApi.Repository
{
    public interface IFileRepository
    {
        public Task<string> AddFile(FilesRequestAddModel fileData , int idUser);
        public List<FilesModel> GetFiles(int idUser, int idGroup, string role , int page , string host);
        public List<FilesModel> GetFilesWithoutGroup(int idUser, string role , int page , string host);
        public string DeleteFiles(int idUser, int idFile);
        public Task<string> UpdateFile(int idUser, FilesRequestUpdateModel filesRequestUpdateModel);
        public string ReservationFile(int idUser, FilesReservationModel filesReservation);
        public Task<string> ReservationFiles(int idUser, List<FilesReservationModel> filesReservation);

    }
}
