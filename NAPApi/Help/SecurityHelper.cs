using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NAPApi.Help
{
    public class SecurityHelper
    {
        private static SecurityHelper securityData;
        private SecurityHelper() { }


        public static SecurityHelper getInstance()
        {
            if (securityData == null)
            {
                securityData = new SecurityHelper();
            }
            return securityData;
        }


        public string getHashPassword(string password)
        {
            //byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); 
            byte[] salt = Encoding.ASCII.GetBytes("Aw7DIn3e+4E+Tta/OAIJTQ==");
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt, // must be unique
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8)
                );

        }

        public string GenerateToken(string username, string email, int id , String role, JwtHelper jwt )
        {

            var Claim = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.PrimarySid, id.ToString()),
                new Claim(ClaimTypes.Email, email),
            };

            DateTime expire = DateTime.Now.AddDays(jwt.DurationExpiredInDay);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    Claim,
                    expires: expire,
                    signingCredentials: signIn
                );
            string finalToken = new JwtSecurityTokenHandler().WriteToken(token);
            return finalToken + "*204*Wesam*" + expire;
        
        }

        public int getIdToken(string Token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(Token);
            foreach(var d in securityToken.Claims)
            {
                if (d.Type.Contains("primarysid")){
                    return Convert.ToInt32(d.Value);
                }
            }
            return -1;
        }
        public int getIdTokenn(string Token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(Token);
            foreach (var d in securityToken.Claims)
            {
                if (d.Type.Contains("primarysid"))
                {
                    return Convert.ToInt32(d.Value);
                }
            }
            return -1;
        }
        public string getRoleToken(string Token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(Token);
            foreach(var d in securityToken.Claims)
            {
                if (d.Type.Contains("role")){
                    return d.Value;
                }
            }
            return "null";
        }

    }
}
