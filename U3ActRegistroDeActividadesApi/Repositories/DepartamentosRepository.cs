using Microsoft.EntityFrameworkCore;
using U3ActRegistroDeActividadesApi.Models.Entities;

namespace U3ActRegistroDeActividadesApi.Repositories
{
    public class DepartamentosRepository(ItesrcneActividadesContext Context) : GenericRepository<Departamentos>(Context)
    {
        public ItesrcneActividadesContext Context = Context;
        public IEnumerable<Departamentos> GetDepartamentos()
        {
            return Context.Departamentos
                .Include(x => x.Actividades)
                .Include(x => x.IdSuperiorNavigation)
                .Include(x => x.InverseIdSuperiorNavigation)
                .OrderBy(x => x.Nombre);
        }

        public Departamentos? GetDepartamentos(int id) => GetAll().FirstOrDefault(x => x.Id == id);
    }
}
