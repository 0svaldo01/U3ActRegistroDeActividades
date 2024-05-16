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
    [Table("Actividades")]
    public class Actividades
    {
        [PrimaryKey]
        public int Id { get; set; }
        [NotNull]
        public string Titulo { get; set; } = null!;
        [NotNull]
        public string? Descripcion { get; set; }
        [NotNull]
        public DateOnly? FechaRealizacion { get; set; }

        public int IdDepartamento { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public int Estado { get; set; }

        public virtual Departamentos IdDepartamentoNavigation { get; set; } = null!;
    }
}
