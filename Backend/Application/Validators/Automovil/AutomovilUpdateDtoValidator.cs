using Application.DTOs.Automovil;
using Application.Json.Converters;
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

            RuleFor(x => x.Color)
                .Must(BeValidOptionalColor).WithMessage("Color no puede estar vacío (usar null para no cambiar) o exceder 30 caracteres");

            RuleFor(x => x.NumeroMotor)
                .Must(BeValidOptionalMotor).WithMessage("Número de motor inválido");
        }

        private bool BeValidOptionalColor(string? color)
        {
            if (color == null) return true;
            
            var normalized = StringSanitizer.NormalizeOrNull(color);
            if (normalized == null) return false;
            
            return normalized.Length <= 30;
        }

        private bool BeValidOptionalMotor(string? numeroMotor)
        {
            if (numeroMotor == null) return true;
            
            var normalized = StringSanitizer.NormalizeOrNull(numeroMotor);
            if (normalized == null) return false;
            
            try { _motorService.Validate(normalized); return true; }
            catch { return false; }
        }
    }
}