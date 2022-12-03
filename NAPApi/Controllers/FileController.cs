using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NAPApi.Help;
using NAPApi.Model;
using NAPApi.Repository;
using System.Net;

namespace NAPApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FileController : Controller
    {
        public readonly IFileRepository fileRepository;
        public FileController(IFileRepository fileRepository)
        {
            this.fileRepository = fileRepository;
        }
        

        [HttpGet("get_files/{idgroup}/{page}")]
        public List<FilesModel> GetGroupFiles([FromHeader] string Authorization, int idGroup , int page)
        {

            if (!ModelState.IsValid)
            {
                return new List<FilesModel>();
            }
            List<FilesModel> files = fileRepository.GetFiles(
                SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]),
                idGroup,
                   SecurityHelper.getInstance().getRoleToken(Authorization.Split(" ")[1]) , page, Request.GetDisplayUrl().Split("/")[2]
                );
            return files;

        }

        [HttpGet("get_files_without_group/{page}")]
        public List<FilesModel> GetFiles([FromHeader] string Authorization, int page)
        {

            if (!ModelState.IsValid)
            {
                return new List<FilesModel>();
            }
            List<FilesModel> files = fileRepository.GetFilesWithoutGroup(
                SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]),
                   SecurityHelper.getInstance().getRoleToken(Authorization.Split(" ")[1]), page,Request.GetDisplayUrl().Split("/")[2]
                );
            return files;

        }


        [HttpPost("add_file")]
        public async Task<IActionResult> AddFiles([FromHeader] string Authorization, [FromForm] FilesRequestAddModel filesRequestAddModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string result = await fileRepository.AddFile(filesRequestAddModel, SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]));
            return Ok(result);
        }

        [HttpDelete("delete_file")]
        public IActionResult DeleteFiles([FromHeader] string Authorization, [FromBody] FilesRequestDeleteModel filesRequestAddModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(fileRepository.DeleteFiles(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]) , filesRequestAddModel.FileId));
        }

        [HttpPut("update_file")]
        public async Task<IActionResult> UpdateFiles([FromHeader] string Authorization, [FromForm] FilesRequestUpdateModel filesRequestUpdateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await fileRepository.UpdateFile(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]), filesRequestUpdateModel));
        }

        [HttpPost("Reservation_File")]
        public IActionResult ReservationFile([FromHeader] string Authorization, [FromBody] FilesReservationModel filesReservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(fileRepository.ReservationFile(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]) , filesReservation) );
        }

        [HttpPost("Reservation_Files")]
        public async Task<IActionResult> ReservationFiles([FromHeader] string Authorization, [FromBody] List<FilesReservationModel> filesReservations)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await fileRepository.ReservationFiles(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]), filesReservations));
        }
    }
}
