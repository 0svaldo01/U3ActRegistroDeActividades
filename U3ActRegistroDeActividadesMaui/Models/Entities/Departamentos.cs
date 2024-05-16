using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableAttribute = SQLite.TableAttribute;

namespace U3ActRegistroDeActividadesMaui.Models.Entities
{
    [Table("Departamentos")]
    public class Departamentos
    {
        [PrimaryKey]
        public int Id { get; set; }
        [NotNull]
        public string Nombre { get; set; } = null!;
        [NotNull]
        public string Username { get; set; } = null!;
        [NotNull]
        public string Password { get; set; } = null!;

        public int? IdSuperior { get; set; }

        public virtual ICollection<Actividades> Actividades { get; set; } = [];

        public virtual Departamentos? IdSuperiorNavigation { get; set; }

        public virtual ICollection<Departamentos> InverseIdSuperiorNavigation { get; set; } = [];
    }
}
