using Microsoft.AspNetCore.Mvc;
using U3ActRegistroDeActividadesApi.Models.DTOs;
using U3ActRegistroDeActividadesApi.Repositories;

namespace U3ActRegistroDeActividadesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadesController(ActividadesRepository Repositorio) : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult GetActividades()
        {
            var actividades = Repositorio.GetActividades();
            if (actividades.Any())
            {
                return Ok(actividades);
            }
            return NotFound("No hay actividades");
        }

        [HttpGet("/{id:int}")]
        public IActionResult GetActividad(int id)
        {
            var actividad = Repositorio.GetActividad(id);
            return actividad != null ? Ok(actividad) : NotFound("No existe la actividad");
        }

        [HttpDelete("/Delete")]
        public IActionResult Eliminar(ActividadDTO dto)
        {
            //ActividadDTOValidator validador = new();
            //var result = validador.Validate(dto);
            //if (result.IsValid)
            //{
            var actividad = Repositorio.GetActividad(dto.Id);
            if (actividad != null)
            {
                actividad.Estado = 0;
                Repositorio.Update(actividad);
                return Ok("Actividad eliminada");
            }
            return NotFound("No existe la actividad que se desea eliminar");
            //}
            //return BadRequest("Selecciona la actividad que desee eliminar");
        }

        [HttpPut("/Update")]
        public IActionResult Editar(ActividadDTO dto)
        {
            return Ok();
        }
    }
}