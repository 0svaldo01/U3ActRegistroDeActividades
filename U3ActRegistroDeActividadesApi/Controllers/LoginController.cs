﻿using Microsoft.AspNetCore.Mvc;
using U3ActRegistroDeActividadesApi.Helpers;
using U3ActRegistroDeActividadesApi.Models.DTOs;
using U3ActRegistroDeActividadesApi.Models.Security;
using U3ActRegistroDeActividadesApi.Models.Validators;
using U3ActRegistroDeActividadesApi.Repositories;

namespace U3ActRegistroDeActividadesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(DepartamentosRepository Repositorio, IConfiguration configuration) : ControllerBase
    {
        [HttpPost("/Login")]
        public IActionResult Login(LoginDTO dto)
        {
            LoginDTOValidator validator = new();
            var result = validator.Validate(dto);
            if (result.IsValid)
            {
                var departamento = Repositorio.GetAll()
                .FirstOrDefault(depto => depto.Username == dto.Username && depto.Password == dto.Password);
                if (departamento != null)
                {
                    string token = "";
                    JWT datos = configuration.GetSection("JWT").Get<JWT>() ?? new();
                    if (!string.IsNullOrWhiteSpace(token = LoginHelper.GetToken(departamento, datos)))
                    {
                        return Ok(token);
                    }
                }
                else
                {
                    return Unauthorized("Usuario no autorizado");
                }
            }
            return BadRequest("Ingresa los datos solicitados");
        }
    }
}
