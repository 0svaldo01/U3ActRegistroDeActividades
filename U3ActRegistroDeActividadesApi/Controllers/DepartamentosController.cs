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
        [HttpGet("/")]
        public IActionResult GetDepartamentos()
        {
            //Obtener todas las actividades de los departamentos
            //var deptos = departamentosRepository.GetDepartamentos().Select(x => x.Actividades.Select(a => a.Titulo));
            //Obtener el nombre de todos los departamentos
            var deptos = departamentosRepository.GetDepartamentos().Select(x => x.Nombre);
            if (deptos != null)
            {
                return Ok(deptos);
            }
            return NotFound("No se han encontrado departamentos");
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
                departamentosRepository.Delete(departamento);
                return Ok();
            }
            return NotFound("No se ah encontrado el departamento");
        }
    }
}
