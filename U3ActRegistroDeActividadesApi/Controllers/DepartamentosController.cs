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
            var deptos = departamentosRepository.GetDepartamentos();
            if (deptos != null)
            {
                return Ok(deptos);
            }
            return NotFound("No se han encontrado departamentos");
        }
    }
}
