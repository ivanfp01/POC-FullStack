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

            RuleFor(x => x.Color)
                .MaximumLength(30).WithMessage("Color no puede exceder 30 caracteres")
                .When(x => x.Color != null);

            RuleFor(x => x.NumeroMotor)
                .Must(BeValidMotor).WithMessage("Número de motor inválido")
                .When(x => !string.IsNullOrEmpty(x.NumeroMotor));
        }

        private bool BeValidMotor(string n)
        {
            try { _motorService.Validate(n); return true; } catch { return false; }
        }
    }
}