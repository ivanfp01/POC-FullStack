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
        public string Tipo { get; set; } = string.Empty;  // ej: Sedan, Pickup, SUV
        public int Anio { get; set; }
        public string Color { get; set; } = string.Empty;

        // Value Object para VIN (NumeroChasis)
        public Vin NumeroChasis { get; set; }

        // Serial de motor (string simple, validado en servicio)
        public string NumeroMotor { get; set; } = string.Empty;

        public DateTime FechaAlta { get; set; } = DateTime.UtcNow;
    }
}