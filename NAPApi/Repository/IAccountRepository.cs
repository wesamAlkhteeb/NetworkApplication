using NAPApi.Model;

namespace NAPApi.Repository
{
    public interface IAccountRepository
    {
        AuthModel Register(RegisterModel registerModel , int type);
        AuthModel Login(LoginModel registerModel);
    }
}
