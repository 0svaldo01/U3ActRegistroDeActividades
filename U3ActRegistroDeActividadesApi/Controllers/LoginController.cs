using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using U3ActRegistroDeActividadesApi.Helpers;
using U3ActRegistroDeActividadesApi.Models.DTOs;
using U3ActRegistroDeActividadesApi.Models.Entities;
using U3ActRegistroDeActividadesApi.Repositories;

namespace U3ActRegistroDeActividadesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IGenericRepository<Departamentos> Repositorio, JWTHelper _helper) : ControllerBase
    {
        [HttpPost]
        public IActionResult Authenticate(LoginDTO dto)
        {
            //Encriptacion de Contraseña
            dto.Password = Encriptacion.EncriptarSHA512(dto.Password);
            var depto = Repositorio.GetAll().FirstOrDefault(x => x.Username == dto.Username && x.Password == dto.Password);
            if (depto == null)
            {
                return Unauthorized();
            }
            var token = _helper.GetToken(depto.Nombre, depto.IdSuperior == null ? "Admin" : "Periodista",
                                [
                                    new Claim("Id", depto.Id.ToString()),
                                    new Claim("IdSuperior",depto.IdSuperior.ToString()??""),
                                    new Claim(ClaimTypes.Name,depto.Nombre),
                                    new Claim(ClaimTypes.Role,depto.IdSuperior>0?"Usuario":"Administrador")
                                ]);
            return Ok(token);
        }
    }
}
