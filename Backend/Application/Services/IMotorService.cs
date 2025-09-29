namespace Application.Services
{
    public interface IMotorService
    {
        /// <summary>
        /// Valida un número de motor
        /// </summary>
        string Validate(string numeroMotor);

        /// <summary>
        /// Genera un número de motor válido basado en marca y año
        /// </summary>
        string Generate(string marca, int anio);
    }
}