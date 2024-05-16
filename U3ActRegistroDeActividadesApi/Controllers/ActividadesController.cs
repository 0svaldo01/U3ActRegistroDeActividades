using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using U3ActRegistroDeActividadesApi.Models.DTOs;
using U3ActRegistroDeActividadesApi.Models.Entities;
using U3ActRegistroDeActividadesApi.Models.Validators;
using U3ActRegistroDeActividadesApi.Repositories;

namespace U3ActRegistroDeActividadesApi.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    //[Authorize(Roles = "Administrador")]
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
        [HttpPost("/Create")]
        public async Task<ActionResult<ActividadDTO>> PostActividad([FromForm] ActividadDTO dto)
        {
            if (dto.Imagen == null || dto.Imagen.Length == 0)
            {
                return BadRequest("Debe subir una imagen.");
            }

            var filePath = Path.Combine($"wwwroot/Images/{dto.Titulo}", Path.GetRandomFileName() + Path.GetExtension(dto.Imagen.FileName));
        
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Imagen.CopyToAsync(stream);
            }

            // Crear la entidad Actividad a partir del DTO
            var actividad = new Actividades
            {
                Id =0,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaRealizacion = dto.FechaRealizacion,
                IdDepartamento = dto.IdDepartamento,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = dto.Estado,
                
            };

            Repositorio.Insert(actividad);
            return CreatedAtAction(nameof(GetActividad), new { id = actividad.Id }, actividad);
        }


        [HttpDelete("/Delete")]
        public IActionResult Eliminar(ActividadDTO dto)
        {
            ActividadDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var actividad = Repositorio.GetActividad(dto.Id);
                if (actividad != null)
                {
                    actividad.Estado = 0;    // Baja lógica
                    actividad.FechaActualizacion = DateTime.UtcNow; //fecha en la que se realizo el cambio

                    Repositorio.Update(actividad);
                    return Ok("Actividad eliminada");
                }
                return NotFound("No existe la actividad que se desea eliminar");
            }
            return BadRequest("Selecciona la actividad que desee eliminar");
        }

        [HttpPut("/Update")]
        public IActionResult Editar(ActividadDTO dto)
        {
            ActividadDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var actividad = Repositorio.GetActividad(dto.Id);
                if (actividad != null)
                {
                    actividad.Estado = 1;
                    actividad.FechaActualizacion = DateTime.UtcNow; //fecha en la que se realizo el cambio
                    actividad.Descripcion = dto.Descripcion;
                    actividad.Titulo = dto.Titulo;
                    actividad.FechaRealizacion = dto.FechaRealizacion;
                    actividad.IdDepartamento = dto.IdDepartamento;

                    Repositorio.Update(actividad);
                    return Ok("Actividad actualizada");
                }
                return NotFound("No existe la actividad que se desea eliminar");
            }
            return BadRequest("Selecciona la actividad que desee eliminar");
        }
    }
}