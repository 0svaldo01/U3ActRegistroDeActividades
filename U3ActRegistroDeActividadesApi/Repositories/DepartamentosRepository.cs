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
            var depa = GetDepartamento(departamentoId);
            if (depa != null)
            {
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
        public void EliminarDepartamento(Departamentos departamento)
        {
            if (departamento != null)
            {
                /** 
                 * Al eliminar un departamento, el departamento no debe tener departamentos subordinados
                 *  por lo que primero se cambiaran todos los departamentos subordinados,
                 *  del departamento que se desea eliminar, para que no haya problemas en la eliminacion
                 */
                foreach (var subdepto in departamento.InverseIdSuperiorNavigation)
                {
                    subdepto.IdSuperior = departamento.IdSuperior ?? 0;
                    Update(subdepto);
                }
                /** 
                 * Ahora que el departamento no tiene subordinados se puede eliminar
                 */
                Delete(departamento);
            }
        }
    }
}