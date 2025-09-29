using Domain.ValueObjects;

namespace Application.Services
{
    public interface IVinService
    {
        /// <summary>
        /// Valida un VIN y devuelve el Value Object Vin o lanza excepción
        /// </summary>
        Vin Validate(string vin);

        /// <summary>
        /// Genera un VIN válido basado en marca, año y tipo
        /// </summary>
        Vin Generate(string marca, int anio, string tipo);
    }
}