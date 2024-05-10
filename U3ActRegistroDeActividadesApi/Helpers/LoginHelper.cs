using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using U3ActRegistroDeActividadesApi.Models.Entities;
using U3ActRegistroDeActividadesApi.Models.Security;

namespace U3ActRegistroDeActividadesApi.Helpers
{
    public class LoginHelper
    {
        private static JWT? JWTConfig;
        public static string GetToken(Departamentos departamento, IConfiguration Configuracion)
        {
            JWTConfig = Configuracion.GetSection("JWT").Get<JWT>() ?? new();
            if (Configuracion != null)
            {
                if (JWTConfig != null && departamento != null)
                {
                    List<Claim> Claims = [
                        new Claim("id",departamento.Id.ToString()),
                        new Claim("idSuperior",departamento.IdSuperior.ToString()??""),
                        new Claim(ClaimTypes.Name, departamento.Nombre)
                        ];

                    JwtSecurityTokenHandler handler = new();
                    var token = handler.CreateToken(new SecurityTokenDescriptor()
                    {
                        Issuer = JWTConfig.Issuer,
                        Audience = JWTConfig.Audiance,
                        IssuedAt = DateTime.UtcNow,
                        Expires = DateTime.UtcNow.AddMinutes(2),
                        SigningCredentials = GetCredentials(),
                        Subject = new ClaimsIdentity(Claims, JwtBearerDefaults.AuthenticationScheme)
                    });
                    var result = handler.WriteToken(token);
                    return result;
                }
            }
            return "";
        }
        private static SigningCredentials GetCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConfig != null ? JWTConfig.Key : ""))
                , SecurityAlgorithms.HmacSha256);
        }
    }
}
