using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAPApi.Help;
using NAPApi.Model;
using NAPApi.Repository;

namespace NAPApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;
        private readonly ILogger<AccountController> logger;

        public AccountController(IAccountRepository accountRepository , ILogger<AccountController> logger)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
        }

        // post https://host/Account/register
        // var a = new AccountController () ;
        // a.Register(..)
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            var result = accountRepository.Register(registerModel, 1);
            if (!result.IsAuthentication)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", result.Message } });
            }
            return Ok(result);
        }

        [NonAction]
        [HttpPost("register_Admin")]
        [Authorize(Roles = "ADMIN")]
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

        [HttpGet("hi")]
        public IActionResult getee()
        {
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", ModelState } });
            }
            var result = accountRepository.Login(loginModel);
            if (!result.IsAuthentication)
            {
                return BadRequest(new Dictionary<string, object>() { { "title", result.Message } });
            }
            return Ok(result);
        }

    }
}
