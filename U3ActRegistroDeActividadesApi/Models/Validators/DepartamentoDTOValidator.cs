using FluentValidation;
using U3ActRegistroDeActividadesApi.Models.DTOs;

namespace U3ActRegistroDeActividadesApi.Models.Validators
{
    public class DepartamentoDTOValidator : AbstractValidator<DepartamentoDTO>
    {
        public DepartamentoDTOValidator()
        {
            //Aqui agregas las condiciones que necesitas
            RuleFor(x => x.Nombre).NotNull().NotEmpty().WithMessage("El nombre del departamento no puede ser nulo o vacio");
        }
    }

}
