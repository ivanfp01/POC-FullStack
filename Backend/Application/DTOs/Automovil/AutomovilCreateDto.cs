namespace Application.DTOs.Automovil
{
    public class AutomovilCreateDto
    {
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string? Color { get; set; }
        public string? NumeroChasis { get; set; }
        public string? NumeroMotor { get; set; }
    }
}