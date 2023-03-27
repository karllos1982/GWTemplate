using System.Security.Claims;
using System.Text;
using GW.Common;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Template.API
{
    public static class TokenService
    {
    
        public const string PRIVATEKEY = "AB725B66-DC4A-40FC-8061-5CFAE0F4C8AB";

        public static AuthToken GenerateToken(string username,string rolename,string instanceid, 
                string permissions, string language, int timeout)
        {
            AuthToken ret = new AuthToken();

            ret.ExpiresDate = DateTime.UtcNow.AddMinutes(timeout);

            var tkhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(PRIVATEKEY);

            Claim cName = new Claim(ClaimTypes.Name, username);
            Claim cRole = new Claim(ClaimTypes.Role, rolename);
            Claim cInstance = new Claim(ClaimTypes.Actor, instanceid);
            Claim cPermissions = new Claim(ClaimTypes.UserData , permissions);
            Claim cLang = new Claim(ClaimTypes.Locality , language);

            Claim[] claims = new Claim[5];
            claims[0] = cName;
            claims[1] = cRole;
            claims[2] = cInstance;
            claims[3] = cPermissions;
            claims[4] = cLang;

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = ret.ExpiresDate,
                SigningCredentials = 
                    new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tkhandler.CreateToken(tokenDesc);

            ret.TokenValue = tkhandler.WriteToken(token); 

            return ret;
        }
    }
}
