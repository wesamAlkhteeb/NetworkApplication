using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAPApi.Context;
using NAPApi.Entity;

namespace NAPApi.Controllers
{
    [Authorize(Roles ="ADMIN")]
    [ApiController]
    [Route("[controller]")]
    public class InitController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        public InitController(ApplicationDbContext applicationDbContext) { 
            this.applicationDbContext = applicationDbContext;
        }

        [NonAction]
        [HttpPost("inital")]
        public IActionResult init()
        {
            applicationDbContext.roles.Add(new Role
            {
                Name = "USER",
            });
            applicationDbContext.SaveChanges();
            applicationDbContext.roles.Add(new Role
            {
                Name= "ADMIN",
            });
            applicationDbContext.SaveChanges();
            applicationDbContext.users.Add(new User
            {
                Confirm = true,
                Email = "Admin@gmail.com",
                Username = "Admin",
                Password = "xffUECTU9s4Rv8OGJpnquUoXnSgAqbJM6Kze4paoDHc=", // "Admin@204
                RoleId = 2,
            });
            applicationDbContext.SaveChanges();
            applicationDbContext.groups.Add(new Group
            {
                UserId = 1,
                GroupName = "Global"
            });
            applicationDbContext.SaveChanges();
            applicationDbContext.permessionsGroups.Add(new PermessionsGroup
            {
                GroupId = 1,
                PermessionsGroupSharedId = 1
            });
            applicationDbContext.SaveChanges();
            return Ok();
        }
    }
}
