using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAPApi.Help;
using NAPApi.Model;
using NAPApi.Repository;

namespace NAPApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GroupController : Controller
    {

        private readonly IGroupRepository groupRepository;

        public GroupController(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        
        [HttpGet("getGroup/{page}")]
        public IActionResult getGroup([FromHeader] string Authorization ,int page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string , object>() { {"title", ModelState } });
            }
            try
            {
                var result = groupRepository.FetchGroups(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]), page, SecurityHelper.getInstance().getRoleToken(Authorization.Split(" ")[1]));
                return Ok(new Dictionary<string,object>()
                {
                    {"title","fetch is success" },
                    {"data" ,result}
                });
            }
            catch(Exception ex) {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
            
        }

        [HttpPost("addGroup")]
        public IActionResult AddGroup([FromHeader] string Authorization , [FromBody] GroupRequestAddtModel groupModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var result = groupRepository.addGroup(
                        groupModel.GroupName,
                        SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]),
                        SecurityHelper.getInstance().getRoleToken(Authorization.Split(" ")[1]));

                return Ok(new Dictionary<string, object>()
                {
                    {"title",result }
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
        }

        [HttpDelete("DeleteGroup")]
        public IActionResult DeleteGroup([FromHeader] string Authorization, [FromBody] GroupRequestDeleteModel groupModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var result = groupRepository.deleteGroup(groupModel.GroupId, SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]));
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



        [HttpPost("GrantPermissionGroup")]
        public IActionResult GrantPermission([FromHeader] string Authorization, [FromBody] GrantPermessionModel Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var result = groupRepository.GrantPermission(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]), Model.IdGroup, Model.UserName); 
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

        [HttpPost("NoGrantPermissionGroup")]
        public IActionResult NoGrantPermission([FromHeader] string Authorization, [FromBody] NoGrantPermessionModel Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var result = groupRepository.NoGrantPermission(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]), Model.IDGroup, Model.IdUserFriend);
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

        [HttpGet("showUsersPermission")]
        public IActionResult FetchUserPermission([FromHeader] string Authorization , int IDGroup , int Page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            try
            {
                var result = groupRepository.FetchUserPermession(SecurityHelper.getInstance().getIdToken(Authorization.Split(" ")[1]), IDGroup, Page);
                return Ok(new Dictionary<string, object>()
                {
                    {"title","Fetch successfully"},
                    {"data",result }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ex.Message } });
            }
            
        }

    }
}