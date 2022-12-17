using NAPApi.Context;
using NAPApi.Help;
using NAPApi.Model;
using NAPApi.Entity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Net;
using NAPApi.state;

namespace NAPApi.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        
        private FileHelper FileHelper;
        public FileRepository(ApplicationDbContext applicationDbContext )
        {
            this.applicationDbContext = applicationDbContext;
           
            FileHelper = new FileHelper();
        }

        public async Task<string> AddFile(FilesRequestAddModel fileData ,int idUser )
        {
            var canAdd = applicationDbContext.groups.
                Where(g => idUser == g.UserId && fileData.IdGroup == g.GroupId).ToList();
            if (canAdd.Count()==0)
            {
                throw new Exception("can't add file to this group");
            }
            var testHas = applicationDbContext.files.
                Where(
                    f => f.FileName == fileData.FileName && 
                         f.GroupId == fileData.IdGroup).Select(c => c.FilesId).ToList();
            if (testHas.Count()>0)
            {
                throw new Exception("can't added , rename file");
            }
            string path = await FileHelper.SaveFile(fileData.File , FileHelper.GenerateName(fileData.File));
            if(path == null)
            {
                throw new Exception("error in save image");
            }
            var file = applicationDbContext.files.Add(new Files
            {
                FileCreateDate = DateTime.Now,
                FileName = fileData.FileName,
                GroupId = fileData.IdGroup,
                FilePath = path,
                FileIdUses = -1
            });
            
            applicationDbContext.SaveChanges();
            applicationDbContext.reports.Add(new Report
            {
                Date= DateTime.Now,
                FileId = file.Entity.FilesId,
                State = new AddStateReport().getState(),
                UserId = idUser
            });
            applicationDbContext.SaveChanges();
            return "Added successfully";
        }

        public string DeleteFiles(int idUser, int idFile)
        {

            var file = (from fl in applicationDbContext.files
                                join gr in applicationDbContext.groups
                                    on fl.GroupId equals gr.GroupId
                                    where gr.UserId == idUser && fl.FilesId ==idFile
                                select fl
                          );

            if (file.Count() >0)
            {
                applicationDbContext.files.Where(f => f.FilesId == idFile).ExecuteDelete();
                applicationDbContext.SaveChanges();
                return "Delete Successfully";
            }
            throw new Exception("error in save image");
        }

        public List<FilesModel> GetFiles (int idUser , int idGroup , string role , int page, string host)
        {
            List<FilesModel> filesModels = new List<FilesModel>();
            var files = (from pr in applicationDbContext.permessionsGroups
                           join fl in applicationDbContext.files
                           on pr.GroupId equals fl.GroupId
                           where fl.GroupId == idGroup && pr.PermessionsGroupSharedId == idUser
                           select new
                           {
                               FileName = fl.FileName,
                               FilePath = fl.FilePath,
                               FileId = fl.FilesId,
                               FileIdUses = fl.FileIdUses!=-1
                           }
                          ).Skip((page - 1) * 10).Take((page - 1) * 10 + 10).ToList();

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    filesModels.Add(new FilesModel
                    {
                        idFile = file.FileId,
                        pathFile= file.FileName,
                        nameFile= "https://"+host+file.FilePath.Replace("\\","/"),
                        isLockFile = file.FileIdUses
                    });
                }
            }
            return filesModels;
        }

        public List<FilesModel> GetFilesWithoutGroup(int idUser, string role, int page, string host)
        {
            List<FilesModel> filesModels = new List<FilesModel>();
            var files = (from pr in applicationDbContext.permessionsGroups
                           join fl in applicationDbContext.files
                           on pr.GroupId equals fl.GroupId
                           where pr.PermessionsGroupSharedId == idUser
                           select new
                           {
                               FileName = fl.FileName,
                               FilePath = fl.FilePath,
                               FileId = fl.FilesId,
                               FileIdUses = fl.FileIdUses !=-1
                           }
                          ).Skip((page - 1) * 10).Take((page - 1) * 10 + 10).ToList();

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    filesModels.Add(new FilesModel
                    {
                        idFile = file.FileId,
                        pathFile = file.FileName,
                        nameFile = "https://" + host + file.FilePath.Replace("\\", "/"),
                        isLockFile = file.FileIdUses
                    });
                }
            }
            return filesModels;
        }

        public string ReservationFile(int idUser, FilesReservationModel filesReservation)
        {
            if (!Reservation(idUser,filesReservation))
            {
                throw new Exception("you can't allow to use this file or The file isn't exists.");
            }
            return "the operation is success";
        }

        public async Task<string> ReservationFiles(int idUser, List<FilesReservationModel> filesReservation)
        {
            


            var transaction = await applicationDbContext.Database.BeginTransactionAsync();
            try
            {
                foreach (var fileR in filesReservation)
                {
                    if (!Reservation(idUser, fileR))
                    {
                        throw new Exception("Can't Reservation files");
                    }
                }
                transaction.Commit();
            }
            catch(Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.ToString());
            }
            return "Reservation done.";
        }
        private bool Reservation(int idUser, FilesReservationModel filesReservation)
        {
            var testHas = (from Pr in applicationDbContext.permessionsGroups
                           join gr in applicationDbContext.groups
                                 on Pr.GroupId equals gr.GroupId
                           join Fl in applicationDbContext.files
                                 on gr.GroupId equals Fl.GroupId
                           where Fl.FilesId == filesReservation.Fileid && Pr.PermessionsGroupSharedId == idUser
                           select new
                           {
                               idUses = Fl.FileIdUses
                           }).ToList();

            if (testHas.Count() == 0)
            {
                return false;
            }
            var Record = applicationDbContext.files.Where(f => f.FilesId == filesReservation.Fileid).ToList();
            if (filesReservation.stateReservation)
            {
                if(Record[0].FileIdUses != -1)
                {
                    return false;
                }
                Record[0].FileIdUses = idUser;
                applicationDbContext.reports.Add(new Report
                {
                    Date = DateTime.Now,
                    FileId = Record[0].FilesId,
                    State = new LockStateReport().getState(),
                    UserId = idUser
                });
            }
            else
            {
                if (Record[0].FileIdUses == -1)
                {
                    return false;
                }
                Record[0].FileIdUses = -1;
                applicationDbContext.reports.Add(new Report
                {
                    Date = DateTime.Now,
                    FileId = Record[0].FilesId,
                    State = new UnLockStateReport().getState(),
                    UserId = idUser
                });
            }
            
            applicationDbContext.SaveChanges();
            return true;
        }

        public async Task<string> UpdateFile(int idUser, FilesRequestUpdateModel filesRequestUpdateModel)
        {
            var file = (from pr in applicationDbContext.permessionsGroups
                                       join fl in applicationDbContext.files
                                       on pr.GroupId equals fl.GroupId
                                       where pr.PermessionsGroupSharedId == idUser && 
                                       fl.FilesId == filesRequestUpdateModel.FileID && 
                                       fl.FileIdUses == idUser
                                       select new
                                       {
                                           FilePath = fl.FilePath,
                                           FileId = fl.FilesId
                                       }
                          ).ToList();

            
            if (file.Count() ==0 )
            {
                throw new Exception("can't update, no allow to update.");
                
            }
            if(!await FileHelper.UpdateFile(file[0].FilePath, filesRequestUpdateModel.File))
            {
                throw new Exception("can't update, the file is not exists. ");
            }
            applicationDbContext.reports.Add(new Report
            {
                Date = DateTime.Now,
                FileId = file[0].FileId,
                State = new UpdateStateReport().getState(),
                UserId = idUser
            });
            applicationDbContext.SaveChanges();
            return "Update successfully";
        }
    }
}
