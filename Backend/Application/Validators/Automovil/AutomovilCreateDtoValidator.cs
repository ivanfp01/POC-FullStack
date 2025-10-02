using Application.DTOs.Automovil;
using Application.Json.Converters;
using Application.Services;
using FluentValidation;

namespace Application.Validators.Automovil
{
    public class AutomovilCreateDtoValidator : AbstractValidator<AutomovilCreateDto>
    {
        private readonly INumeroChasisService _chasisService;
        private readonly IMotorService _motorService;

        public AutomovilCreateDtoValidator(INumeroChasisService chasisService, IMotorService motorService)
        {
            _chasisService = chasisService;
            _motorService = motorService;

            RuleFor(x => x.Marca)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Marca es requerida")
                .NotEmpty().WithMessage("Marca no puede estar vacía")
                .Must(x => !string.IsNullOrWhiteSpace(x) && !StringSanitizer.IsPlaceholder(x)).WithMessage("Marca no puede ser un placeholder")
                .MaximumLength(60).WithMessage("Marca no puede exceder 60 caracteres");

            RuleFor(x => x.Modelo)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Modelo es requerido")
                .NotEmpty().WithMessage("Modelo no puede estar vacío")
                .Must(x => !string.IsNullOrWhiteSpace(x) && !StringSanitizer.IsPlaceholder(x)).WithMessage("Modelo no puede ser un placeholder")
                .MaximumLength(60).WithMessage("Modelo no puede exceder 60 caracteres");

            RuleFor(x => x.Año)
                .Cascade(CascadeMode.Stop)
                .InclusiveBetween(1900, DateTime.Now.Year + 1).WithMessage($"Año debe estar entre 1900 y {DateTime.Now.Year + 1}");

            RuleFor(x => x.Color)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Color es requerido")
                .NotEmpty().WithMessage("Color no puede estar vacío")
                .Must(x => !string.IsNullOrWhiteSpace(x) && !StringSanitizer.IsPlaceholder(x)).WithMessage("Color no puede ser un placeholder")
                .MaximumLength(30).WithMessage("Color no puede exceder 30 caracteres");

            RuleFor(x => x.NumeroChasis)
                .Must(BeValidOptionalChasis).WithMessage("Número de chasis (VIN) inválido");

            RuleFor(x => x.NumeroMotor)
                .Must(BeValidOptionalMotor).WithMessage("Número de motor inválido");
        }

        private bool BeValidOptionalChasis(string? vin)
        {
            var normalized = StringSanitizer.NormalizeOrNull(vin);
            if (normalized == null) return true;
            
            try { _chasisService.Validate(normalized); return true; }
            catch { return false; }
        }

        private bool BeValidOptionalMotor(string? numeroMotor)
        {
            var normalized = StringSanitizer.NormalizeOrNull(numeroMotor);
            if (normalized == null) return true;
            
            try { _motorService.Validate(normalized); return true; }
            catch { return false; }
        }
    }
}