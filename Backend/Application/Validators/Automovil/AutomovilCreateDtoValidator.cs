using Application.DTOs.Automovil;
using Application.Services;
using FluentValidation;

namespace Application.Validators.Automovil
{
    public class AutomovilCreateDtoValidator : AbstractValidator<AutomovilCreateDto>
    {
        private readonly INumeroChasisService _numeroChasisService;
        private readonly IMotorService _motorService;

        public AutomovilCreateDtoValidator(INumeroChasisService numeroChasisService, IMotorService motorService)
        {
            _numeroChasisService = numeroChasisService;
            _motorService = motorService;

            RuleFor(x => x.Marca)
                .NotEmpty().WithMessage("Marca es requerida")
                .MaximumLength(60).WithMessage("Marca no puede exceder 60 caracteres");

            RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage("Modelo es requerido")
                .MaximumLength(60).WithMessage("Modelo no puede exceder 60 caracteres");

            RuleFor(x => x.Anio)
                .GreaterThanOrEqualTo(1900).WithMessage("Año debe ser mayor o igual a 1900")
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage($"Año no puede ser mayor a {DateTime.UtcNow.Year + 1}");

            RuleFor(x => x.Color)
                .MaximumLength(30).WithMessage("Color no puede exceder 30 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Color));

            RuleFor(x => x.NumeroChasis)
                .Must(BeValidNumeroChasis).WithMessage("Número de chasis (VIN) inválido")
                .When(x => !string.IsNullOrEmpty(x.NumeroChasis));

            RuleFor(x => x.NumeroMotor)
                .Must(BeValidMotorNumber).WithMessage("Número de motor inválido")
                .When(x => !string.IsNullOrEmpty(x.NumeroMotor));
        }

        private bool BeValidNumeroChasis(string numeroChasis)
        {
            try
            {
                _numeroChasisService.Validate(numeroChasis);
                return true;
            }
            catch
            {
                return false;
            }
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