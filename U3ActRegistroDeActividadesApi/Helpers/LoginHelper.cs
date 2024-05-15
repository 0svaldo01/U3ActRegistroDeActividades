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
        public static string GetToken(Departamentos departamento, JWT Configuracion)
        {
            if (Configuracion != null && departamento != null)
            {
                List<Claim> Claims =
                [
                    new Claim("id",departamento.Id.ToString()),
                    new Claim("idSuperior",departamento.IdSuperior.HasValue.ToString()??"0"),
                    new Claim(ClaimTypes.Name, departamento.Nombre),
                    new Claim(ClaimTypes.Role,departamento.IdSuperior!=null?"Usuario":"Administrador")
                ];

                JwtSecurityTokenHandler handler = new();
                var SigningCredential = new SigningCredentials
                                         (
                                             new SymmetricSecurityKey
                                             (
                                                Encoding.UTF8.GetBytes(Configuracion.Key)
                                             ),
                                             SecurityAlgorithms.HmacSha512
                                         );
                var token = handler.CreateToken(new SecurityTokenDescriptor()
                {
                    Issuer = Configuracion.Issuer,
                    Audience = Configuracion.Audiance,
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = SigningCredential,
                    Subject = new ClaimsIdentity(Claims, JwtBearerDefaults.AuthenticationScheme)
                });
                return handler.WriteToken(token);
            }
            return "";
        }
    }
}