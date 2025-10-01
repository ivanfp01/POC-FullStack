using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public record NumeroChasisVo
    {
        private static readonly Regex VinRegex = new(@"^[A-HJ-NPR-Z0-9]{17}$", RegexOptions.Compiled);

        public string Value { get; }

        private NumeroChasisVo(string value)
        {
            Value = value;
        }

        public static NumeroChasisVo Create(string numeroChasis)
        {
            if (string.IsNullOrWhiteSpace(numeroChasis))
                throw new ArgumentException("Número de chasis (VIN) requerido");

            numeroChasis = numeroChasis.Trim().ToUpperInvariant();

            if (!VinRegex.IsMatch(numeroChasis))
                throw new ArgumentException("Número de chasis (VIN) inválido. Debe tener 17 caracteres y no contener I/O/Q.");

            if (!CheckDigitIsValid(numeroChasis))
                throw new ArgumentException("Número de chasis (VIN) con dígito verificador inválido.");

            return new NumeroChasisVo(numeroChasis);
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