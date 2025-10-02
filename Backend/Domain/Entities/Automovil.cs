using Domain.ValueObjects;

namespace Domain.Entities
{
    /// <summary>
    /// Entidad Automóvil completa con Value Objects y validaciones
    /// </summary>
    public class Automovil
    {
        public int Id { get; set; }

        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Año { get; set; }
        public string? Color { get; set; }

        // Value Object para número de chasis (VIN)
        public NumeroChasisVo NumeroChasis { get; set; } = default!;

        // Serial de motor (string simple, validado en servicio)
        public string NumeroMotor { get; set; } = string.Empty;

        public DateTime FechaAlta { get; set; } = DateTime.Now;
    }
}