using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAPApi.Help;
using NAPApi.Repository;

namespace NAPApi.Controllers
{
    [Authorize(Roles ="ADMIN")]
    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly IReportRepository report;
        public ReportController(IReportRepository report)
        {
            this.report = report;
        }
        [HttpGet("byname")]
        public IActionResult GetReprotbyUser([FromHeader] string Authorization ,string Username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var reports = report.getReportByUser(Username);
                return Ok(new Dictionary<string, object>()
                {
                    {"title","fetch is success" },
                    {"reports" ,reports}
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
        }
        
        [HttpGet("byfile")]
        public IActionResult GetReprotbyFile([FromHeader] string Authorization, int FileId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var reports = report.getReportByFile(FileId);
                return Ok(new Dictionary<string, object>()
                {
                    {"title","fetch is success" },
                    {"reports" ,reports}
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
        }

    }
}
