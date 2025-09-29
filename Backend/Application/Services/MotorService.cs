using System.Text.RegularExpressions;

namespace Application.Services
{
    public class MotorService : IMotorService
    {
        private static readonly Regex MotorRegex = new(@"^[A-Z0-9]{7,12}$", RegexOptions.Compiled);
        private static readonly Dictionary<string, string> MarcaToPrefix = new(StringComparer.OrdinalIgnoreCase)
        {
            { "FORD", "FRD" }, { "CHEVROLET", "CHV" }, { "TOYOTA", "TOY" },
            { "HONDA", "HND" }, { "NISSAN", "NIS" }, { "HYUNDAI", "HYU" },
            { "KIA", "KIA" }, { "BMW", "BMW" }, { "MERCEDES", "MER" },
            { "AUDI", "AUD" }, { "VOLKSWAGEN", "VWG" }, { "PEUGEOT", "PEU" },
            { "RENAULT", "REN" }, { "FIAT", "FIA" }, { "DEFAULT", "GEN" }
        };

        private static int _motorSequence = 1000;

        public string Validate(string numeroMotor)
        {
            if (string.IsNullOrWhiteSpace(numeroMotor))
                throw new ArgumentException("Número de motor requerido");

            numeroMotor = numeroMotor.Trim().ToUpperInvariant();

            if (!MotorRegex.IsMatch(numeroMotor))
                throw new ArgumentException("Número de motor inválido. Debe tener entre 7-12 caracteres alfanuméricos.");

            return numeroMotor;
        }

        public string Generate(string marca, int anio)
        {
            // Prefijo de marca (3 caracteres)
            string prefix = MarcaToPrefix.GetValueOrDefault(marca.ToUpper(), MarcaToPrefix["DEFAULT"]);

            // Año (2 dígitos)
            string anioStr = (anio % 100).ToString("D2");

            // Secuencia incremental
            var sequence = Interlocked.Increment(ref _motorSequence);

            // Número de motor: PREFIJO + AÑO + SECUENCIA
            string numeroMotor = $"{prefix}{anioStr}{sequence:D6}";

            // Asegurar que no exceda 12 caracteres
            if (numeroMotor.Length > 12)
                numeroMotor = numeroMotor.Substring(0, 12);

            return numeroMotor;
        }
    }
}