using Microsoft.AspNetCore.Mvc;

namespace U3ActRegistroDeActividadesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController(Repositories.DepartamentosRepository departamentosRepository) : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult GetDepartamentos()
        {
            var departamentos = departamentosRepository.GetDepartamentos();
            return departamentos != null ? Ok(departamentos) : NotFound("No hay departamentos existentes");
        }
    }
}
