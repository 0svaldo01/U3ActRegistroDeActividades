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
            var depa = GetDepartamentos(departamentoId);
            if (depa != null)
            {
                //crear un dto
                DeptoDTO depto = new()
                {
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
                    Subordinados = depa.InverseIdSuperiorNavigation
                                .Select(subdep => GetActividadesRecursivasPorDepartamento(subdep.Id))
                };
                return depto;
            }
            return new();
        }

        public Departamentos? GetDepartamentos(int id) => GetDepartamentos().FirstOrDefault(x => x.Id == id);
    }
}
