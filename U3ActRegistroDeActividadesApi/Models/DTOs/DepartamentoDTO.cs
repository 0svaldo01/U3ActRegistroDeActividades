using U3ActRegistroDeActividadesApi.Models.Entities;

namespace U3ActRegistroDeActividadesApi.Models.DTOs
{
    public class DepartamentoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int? IdSuperior { get; set; }
        public virtual ICollection<Actividades> Actividades { get; set; } = [];
        public virtual ICollection<Departamentos> InverseIdSuperiorNavigation { get; set; } = [];
    }
}
