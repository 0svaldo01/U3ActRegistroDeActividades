using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace U3ActRegistroDeActividadesApi.Helpers
{
    public class JWTHelper(IConfiguration conf)
    {
        public string GetToken(string username, string role, List<Claim> claims)
        {
            JwtSecurityTokenHandler handler = new();
            var issuer = conf.GetSection("JWT").GetValue<string>("Issuer") ?? "";
            var secret = conf.GetSection("JWT").GetValue<string>("Key");
            var audience = conf.GetSection("JWT").GetValue<string>("Audiance") ?? "";

            List<Claim> basicas = [
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Iss , issuer),
                new Claim(JwtRegisteredClaimNames.Aud, audience),
            ];

            basicas.AddRange(claims);
            //new SigningCredentials
            JwtSecurityToken jwtSecurity =
                new(issuer, audience, basicas, DateTime.Now, DateTime.Now.AddMinutes(20),
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? "")),
                SecurityAlgorithms.HmacSha256));
            return handler.WriteToken(jwtSecurity);
        }
    }
}
