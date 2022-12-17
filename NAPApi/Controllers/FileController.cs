using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NAPApi.Help;
using NAPApi.Model;
using NAPApi.Repository;

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

        [HttpGet("get_files")]
        public IActionResult GetGroupFiles([FromHeader] string Authorization, int idGroup, int page)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                List<FilesModel> files = fileRepository.GetFiles(
                    SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]),
                    idGroup,
                    SecurityHelper.getInstance().getRoleToken(Authorization.Split(" ")[1]),
                    page,
                    Request.GetDisplayUrl().Split("/")[2]
                );
                // getfile ( token , idgroup , role )
                return Ok(new Dictionary<string, object>()
                {
                    {"title","fetch is success" },
                    {"data" ,files}
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
        }

        [HttpGet("get_files_without_group/{page}")]
        public IActionResult GetFiles([FromHeader] string Authorization, int page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                List<FilesModel> files = fileRepository.GetFilesWithoutGroup(
                SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]),
                   SecurityHelper.getInstance().getRoleToken(Authorization.Split(" ")[1]), page, Request.GetDisplayUrl().Split("/")[2]
                );
                return Ok(new Dictionary<string, object>()
                {
                    {"title","fetch is success" },
                    {"data" ,files}
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
        }


        [HttpPost("add_file")]
        public async Task<IActionResult> AddFiles([FromHeader] string Authorization, [FromForm] FilesRequestAddModel filesRequestAddModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                string result = await fileRepository.AddFile(filesRequestAddModel, SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]));
                return Ok(new Dictionary<string, object>()
                {
                    {"title",result }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
        }

        [HttpDelete("delete_file")]
        public IActionResult DeleteFiles([FromHeader] string Authorization, [FromBody] FilesRequestDeleteModel filesRequestAddModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var result = fileRepository.DeleteFiles(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]), filesRequestAddModel.FileId);
                return Ok(new Dictionary<string, object>()
                {
                    {"title",result }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }

        }

        [HttpPut("update_file")]
        public async Task<IActionResult> UpdateFiles([FromHeader] string Authorization, [FromForm] FilesRequestUpdateModel filesRequestUpdateModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var result = await fileRepository.UpdateFile(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]), filesRequestUpdateModel);
                return Ok(new Dictionary<string, object>()
                {
                    { "title", result }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
        }

        [HttpPost("Reservation_File")]
        public IActionResult ReservationFile([FromHeader] string Authorization, [FromBody] FilesReservationModel filesReservation)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var result = fileRepository.ReservationFile(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]), filesReservation);
                return Ok(new Dictionary<string, object>()
                {
                    { "title", result }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
        }

        [HttpPost("Reservation_Files")]
        public async Task<IActionResult> ReservationFiles([FromHeader] string Authorization, [FromBody] List<FilesReservationModel> filesReservations)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var result = await fileRepository.ReservationFiles(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]), filesReservations);
                return Ok(new Dictionary<string, object>()
                {
                    { "title", result }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
        }
    }
}
