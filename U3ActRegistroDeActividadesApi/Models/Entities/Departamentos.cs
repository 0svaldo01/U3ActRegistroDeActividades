namespace U3ActRegistroDeActividadesApi.Models.Entities;

public partial class Departamentos
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? IdSuperior { get; set; }

    public virtual ICollection<Actividades> Actividades { get; set; } = [];

    public virtual Departamentos? IdSuperiorNavigation { get; set; }

    public virtual ICollection<Departamentos> InverseIdSuperiorNavigation { get; set; } = [];
}
