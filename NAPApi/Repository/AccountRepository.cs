using Microsoft.Extensions.Options;
using NAPApi.Context;
using NAPApi.Help;
using NAPApi.Model;
using System.IdentityModel.Tokens.Jwt;
using NAPApi.Entity;

namespace NAPApi.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly JwtHelper _jwt;

        public AccountRepository(ApplicationDbContext applicationDbContext , IOptions<JwtHelper> jwt)
        {
            this.applicationDbContext = applicationDbContext;
            this._jwt = jwt.Value;
        }

        public AuthModel Login(LoginModel loginModel)
        {

            var user = applicationDbContext.users.Where(
                u => 
                    u.Email == loginModel.Email && 
                    u.Password == SecurityHelper.getInstance().getHashPassword(loginModel.Password)
                ).Join(
                    applicationDbContext.roles,
                    user => user.RoleId,
                    role => role.Id,
                    (user,role)=>new
                    {
                        Username = user.Username,
                        Email= user.Email,
                        role = role.Name,
                        UserId = user.Id

                    }
                ).ToList();

            if(user.Count() == 0)
            {
                return new AuthModel
                {
                    IsAuthentication = false,
                    Message = "email or password is incurrect"
                };
            }

            String token = SecurityHelper.getInstance().GenerateToken(user[0].Username, user[0].Email , user[0].UserId , user[0].role, _jwt);
            return new AuthModel
            {
                Message = "login success",
                Email = user[0].Email,
                Username = user[0].Username,
                IsAuthentication = true,
                Roles = user[0].role,
                Token = token.Split("*204*Wesam*")[0],
                ExpiresOn = Convert.ToDateTime(token.Split("*204*Wesam*")[1])
            };
        }

        public AuthModel Register(RegisterModel registerModel , int type)
        {
            if(IsEmailExists(registerModel.Email))
            {
                return new AuthModel
                {
                    IsAuthentication = false,
                    Message = "Email is Exists"
                };
            }
            if (IsUserNameExists(registerModel.Username))
            {
                return new AuthModel
                {
                    IsAuthentication = false,
                    Message = "Username is Exists"
                };
            }
            if (!ValidateInputHelper.getInstance().isValidEmail(registerModel.Email))
            {
                return new AuthModel
                {
                    IsAuthentication = false ,
                    Message = "Error in form Email"
                };
            }
            if(registerModel.Username.Length<5 || registerModel.Username.Length >= 60)
            {
                return new AuthModel
                {
                    IsAuthentication = false,
                    Message = "username length must be between 5 - 60 characters"
                };
            }
            if (!ValidateInputHelper.getInstance().isValidPassword(registerModel.Password)  || registerModel.Password.Length<8 )
            {
                return new AuthModel
                {
                    IsAuthentication = false,
                    Message = "password must contain capital and small letters and digit and symbol"
                };
            }
            applicationDbContext.users.Add(new User
            {
                Username = registerModel.Username,
                Password = SecurityHelper.getInstance().getHashPassword(registerModel.Password),
                Email = registerModel.Email,
                Confirm = true,
                RoleId = type
            });
            applicationDbContext.SaveChanges();
            var idUser = applicationDbContext.users.Where(u => u.Email == registerModel.Email).Select(u => u.Id).ToList();
            var idGroup = applicationDbContext.groups.Where(u => u.GroupName == "Global").Select(u => u.GroupId).ToList();
            
            applicationDbContext.permessionsGroups.Add(new PermessionsGroup
            {
                GroupId = idGroup[0],
                PermessionsGroupSharedId = idUser[0],
            });
            applicationDbContext.SaveChanges();
            return new AuthModel
            {
                Message = "register success",
                Email = registerModel.Email,
                Username = registerModel.Username,
                IsAuthentication = true,
                Roles = "USER",
            };
        }

        

        private bool IsEmailExists (string Email)
        {
            var user = applicationDbContext.users.Where(e => e.Email == Email).ToList();
            return user.Count() > 0;
        }


        private bool IsUserNameExists(string Username)
        {
            var user = applicationDbContext.users.Where(e => e.Username == Username).ToList();
            return user.Count() > 0;
        }

    }
}
