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
                return BadRequest(ModelState);
            }
            var reports = report.getReportByUser(Username);
            return Ok(reports);
        }
        
        [HttpGet("byfile")]
        public IActionResult GetReprotbyFile([FromHeader] string Authorization, int FileId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reports = report.getReportByFile(FileId);
            return Ok(reports);
        }

    }
}
