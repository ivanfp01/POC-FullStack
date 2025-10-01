using Domain.ValueObjects;

namespace Application.Services
{
    public interface INumeroChasisService
    {
        /// <summary>
        /// Valida un número de chasis (VIN) y devuelve el Value Object o lanza excepción
        /// </summary>
        NumeroChasisVo Validate(string numero);

        /// <summary>
        /// Genera un número de chasis (VIN) válido basado en marca y año
        /// </summary>
        NumeroChasisVo Generate(string marca, int anio);
    }
}