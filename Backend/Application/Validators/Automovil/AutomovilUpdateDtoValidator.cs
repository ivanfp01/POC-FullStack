using Application.DTOs.Automovil;
using Application.Services;
using FluentValidation;

namespace Application.Validators.Automovil
{
    public class AutomovilUpdateDtoValidator : AbstractValidator<AutomovilUpdateDto>
    {
        private readonly IMotorService _motorService;

        public AutomovilUpdateDtoValidator(IMotorService motorService)
        {
            _motorService = motorService;

            RuleFor(x => x.Marca)
                .NotEmpty().WithMessage("Marca es requerida")
                .MaximumLength(60).WithMessage("Marca no puede exceder 60 caracteres")
                .When(x => x.Marca != null);

            RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage("Modelo es requerido")
                .MaximumLength(60).WithMessage("Modelo no puede exceder 60 caracteres")
                .When(x => x.Modelo != null);

            RuleFor(x => x.Anio)
                .GreaterThanOrEqualTo(1900).WithMessage("Año debe ser mayor o igual a 1900")
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage($"Año no puede ser mayor a {DateTime.UtcNow.Year + 1}")
                .When(x => x.Anio.HasValue);

            RuleFor(x => x.Color)
                .MaximumLength(30).WithMessage("Color no puede exceder 30 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Color));

            RuleFor(x => x.NumeroMotor)
                .Must(BeValidMotorNumber).WithMessage("Número de motor inválido")
                .When(x => !string.IsNullOrEmpty(x.NumeroMotor));
        }

        private bool BeValidMotorNumber(string numeroMotor)
        {
            try
            {
                _motorService.Validate(numeroMotor);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}