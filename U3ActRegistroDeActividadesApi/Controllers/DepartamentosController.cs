using Microsoft.AspNetCore.Mvc;
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
    }
}
