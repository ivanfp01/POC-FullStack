namespace Application.DTOs.Automovil
{
    public class AutomovilCreateDto
    {
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Año { get; set; }
        public string Color { get; set; } = string.Empty;
        public string? NumeroChasis { get; set; }
        public string? NumeroMotor { get; set; }
    }
}