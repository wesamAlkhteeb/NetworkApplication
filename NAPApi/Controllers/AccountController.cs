using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NAPApi.Model;
using NAPApi.Repository;
using System.Net;

namespace NAPApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = accountRepository.Register(registerModel , 1);
            if (!result.IsAuthentication)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("register_Admin")]
        [Authorize(Roles ="ADMIN")]
        public IActionResult RegisterAdmin([FromBody] RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = accountRepository.Register(registerModel, 2);
            if (!result.IsAuthentication)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = accountRepository.Login(loginModel);
            if (!result.IsAuthentication)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

    }
}
