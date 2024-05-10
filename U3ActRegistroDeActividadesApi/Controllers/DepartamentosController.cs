using Microsoft.AspNetCore.Mvc;
using U3ActRegistroDeActividadesApi.Models.DTOs;
using U3ActRegistroDeActividadesApi.Models.Validators;
using U3ActRegistroDeActividadesApi.Repositories;

namespace U3ActRegistroDeActividadesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController(DepartamentosRepository departamentosRepository) : ControllerBase
    {
        [HttpGet("/Departamento/{id:int}")]
        public IActionResult GetDepartamentoActividades(int id)
        {
            var depto = departamentosRepository.GetActividadesRecursivasPorDepartamento(id);
            return Ok(depto);
        }

        //Ejemplo de uso de validadores con fluentvalidation
        [HttpPost]
        public IActionResult Agregar(DepartamentoDTO dto)
        {
            DepartamentoDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("/Eliminar/{id:int}")]
        public IActionResult Eliminar(int id)
        {
            var departamento = departamentosRepository.GetById(id);
            if (departamento != null)
            {
                departamentosRepository.EliminarDepartamento(departamento);
                return Ok("Se ah eliminado el departamento");
            }
            return NotFound("No se ah encontrado el departamento");
        }
    }
}
