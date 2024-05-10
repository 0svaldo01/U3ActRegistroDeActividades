using Microsoft.EntityFrameworkCore;
using U3ActRegistroDeActividadesApi.Models.DTOs;
using U3ActRegistroDeActividadesApi.Models.Entities;

namespace U3ActRegistroDeActividadesApi.Repositories
{
    public class DepartamentosRepository(ItesrcneActividadesContext Context) : GenericRepository<Departamentos>(Context)
    {
        public ItesrcneActividadesContext Context = Context;
        public IEnumerable<Departamentos> GetDepartamentos()
        {
            var depas = Context.Departamentos
                .Include(x => x.Actividades)
                .Include(x => x.InverseIdSuperiorNavigation);
            return depas;
        }
        public DeptoDTO GetActividadesRecursivasPorDepartamento(int departamentoId)
        {
            //Obtener el departamento raiz
            var depa = GetDepartamento(departamentoId);
            if (depa != null)
            {
                //crear un dto
                DeptoDTO depto = new()
                {
                    Id = depa.Id,
                    Departamento = depa.Nombre,
                    Actividades = depa.Actividades.Select(a => new ActividadDTO
                    {
                        Descripcion = a.Descripcion,
                        FechaActualizacion = a.FechaActualizacion,
                        Estado = a.Estado,
                        FechaCreacion = a.FechaCreacion,
                        FechaRealizacion = a.FechaRealizacion,
                        IdDepartamento = a.IdDepartamento,
                        Titulo = a.Titulo
                    }),
                    Subordinados = depa.InverseIdSuperiorNavigation.Select(subdep => GetActividadesRecursivasPorDepartamento(subdep.Id))
                };
                return depto;
            }
            //Es necesario regresar un nuevo objeto debido a que los subordinados no puede ser null
            //En el body del response se veria asi []
            return new();
        }
        public Departamentos? GetDepartamento(int id) => GetDepartamentos().FirstOrDefault(x => x.Id == id);
        public bool EliminarDepartamento(int id)
        {
            //Buscamos el departamentos raiz
            var departamento = GetDepartamento(id);
            if (departamento != null)
            {
                //Cambiamos el IdSuperior los subordinados directos
                foreach (var subordinado in departamento.InverseIdSuperiorNavigation)
                {
                    subordinado.IdSuperior = 0;
                    //Actualizamos el subordinado
                    Update(subordinado);
                }
                // Ahora que el departamento no tiene subordinados se puede eliminar
                Delete(departamento);
                return true;
            }
            return false;
        }

    }
}
