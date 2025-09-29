namespace Domain.Entities
{
    /// <summary>
    /// Entidad Automovil - Stub temporal para migración inicial
    /// Esta será completada en el próximo prompt con Value Objects y validaciones
    /// </summary>
    public class Automovil
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string Color { get; set; } = string.Empty;
        public string NumeroChasis { get; set; } = string.Empty;
        public string NumeroMotor { get; set; } = string.Empty;
        public DateTime FechaAlta { get; set; }
    }
}