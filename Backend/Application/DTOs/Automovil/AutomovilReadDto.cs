namespace Application.DTOs.Automovil
{
    public class AutomovilReadDto
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string? Color { get; set; }
        public string NumeroChasis { get; set; } = string.Empty;
        public string NumeroMotor { get; set; } = string.Empty;
        public DateTime FechaAlta { get; set; }
    }
}