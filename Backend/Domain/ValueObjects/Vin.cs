using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public record Vin
    {
        private static readonly Regex VinRegex = new(@"^[A-HJ-NPR-Z0-9]{17}$", RegexOptions.Compiled);

        public string Value { get; }

        private Vin(string value)
        {
            Value = value;
        }

        public static Vin Create(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                throw new ArgumentException("VIN requerido");

            vin = vin.Trim().ToUpperInvariant();

            if (!VinRegex.IsMatch(vin))
                throw new ArgumentException("VIN inválido. Debe tener 17 caracteres y no contener I/O/Q.");

            if (!CheckDigitIsValid(vin))
                throw new ArgumentException("Dígito verificador inválido.");

            return new Vin(vin);
        }

        private static bool CheckDigitIsValid(string vin)
        {
            // Implementación estándar ISO 3779
            const string map = "0123456789.ABCDEFGH..JKLMN.P.R..STUVWXYZ";
            int[] weights = {8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2};

            int sum = 0;
            for (int i = 0; i < vin.Length; i++)
            {
                int value = map.IndexOf(vin[i]) % 10;
                sum += value * weights[i];
            }

            char expectedCheckDigit = (sum % 11 == 10) ? 'X' : (char)('0' + (sum % 11));
            return vin[8] == expectedCheckDigit;
        }

        public override string ToString() => Value;
    }
}