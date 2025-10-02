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
                .MaximumLength(60).WithMessage("Marca no puede exceder 60 caracteres")
                .Must(v => !IsPlaceholder(v)).WithMessage("Marca es inválida");

            RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage("Modelo es requerido")
                .MaximumLength(60).WithMessage("Modelo no puede exceder 60 caracteres")
                .Must(v => !IsPlaceholder(v)).WithMessage("Modelo es inválido");

            RuleFor(x => x.Año)
                .GreaterThanOrEqualTo(1900).WithMessage("Año debe ser mayor o igual a 1900")
                .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage($"Año no puede ser mayor a {DateTime.Now.Year + 1}");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Color es requerido")
                .MaximumLength(30).WithMessage("Color no puede exceder 30 caracteres")
                .Must(v => !IsPlaceholder(v)).WithMessage("Color es inválido");

            RuleFor(x => x.NumeroChasis)
                .Must(BeValidChasis).WithMessage("Número de chasis (VIN) inválido")
                .When(x => !string.IsNullOrWhiteSpace(x.NumeroChasis) && !IsPlaceholder(x.NumeroChasis));

            RuleFor(x => x.NumeroMotor)
                .Must(BeValidMotor).WithMessage("Número de motor inválido")
                .When(x => !string.IsNullOrWhiteSpace(x.NumeroMotor) && !IsPlaceholder(x.NumeroMotor));
        }

        private bool BeValidChasis(string vin)
        {
            try { _numeroChasisService.Validate(vin); return true; } catch { return false; }
        }

        private bool BeValidMotor(string n)
        {
            try { _motorService.Validate(n); return true; } catch { return false; }
        }

        // Helper
        private static bool IsPlaceholder(string? s) =>
            string.Equals(s?.Trim(), "string", StringComparison.OrdinalIgnoreCase);
    }
}