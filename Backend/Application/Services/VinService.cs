using Domain.ValueObjects;

namespace Application.Services
{
    public class VinService : IVinService
    {
        private static readonly Dictionary<string, string> MarcaToWMI = new(StringComparer.OrdinalIgnoreCase)
        {
            { "FORD", "1FA" }, { "CHEVROLET", "1GC" }, { "TOYOTA", "4T1" },
            { "HONDA", "JHM" }, { "NISSAN", "JN1" }, { "HYUNDAI", "KMH" },
            { "KIA", "KNA" }, { "BMW", "WBA" }, { "MERCEDES", "WDD" },
            { "AUDI", "WAU" }, { "VOLKSWAGEN", "WVW" }, { "PEUGEOT", "VF3" },
            { "RENAULT", "VF1" }, { "FIAT", "ZFA" }, { "DEFAULT", "1XY" }
        };

        private static int _sequenceCounter = 100000;

        public Vin Validate(string vin)
        {
            return Vin.Create(vin);
        }

        public Vin Generate(string marca, int anio, string tipo)
        {
            // WMI (World Manufacturer Identifier) - 3 caracteres
            string wmi = MarcaToWMI.GetValueOrDefault(marca.ToUpper(), MarcaToWMI["DEFAULT"]);

            // VDS (Vehicle Descriptor Section) - 5 caracteres
            string vds = GenerateVDS(tipo, marca);

            // Incrementar contador de secuencia
            var sequence = Interlocked.Increment(ref _sequenceCounter);

            // Check digit placeholder (posición 9)
            string vinWithoutCheck = wmi + vds + "0";

            // VIS (Vehicle Identifier Section) - 8 caracteres (incluyendo año)
            char anioCodigo = GetAnioCodigo(anio);
            string vis = $"{anioCodigo}A{sequence:D6}".Substring(0, 8);

            // VIN completo sin dígito verificador
            string vinSinCheck = vinWithoutCheck + vis;

            // Calcular dígito verificador
            char checkDigit = CalculateCheckDigit(vinSinCheck);

            // VIN final con dígito verificador en posición 8
            string vinFinal = vinSinCheck.Substring(0, 8) + checkDigit + vinSinCheck.Substring(9);

            return Vin.Create(vinFinal);
        }

        private static string GenerateVDS(string tipo, string marca)
        {
            // Generar VDS basado en tipo de vehículo
            return tipo.ToUpper() switch
            {
                "SEDAN" => "A1B2C",
                "SUV" => "D3E4F",
                "PICKUP" => "G5H6J",
                "HATCHBACK" => "K7L8M",
                _ => "N9P0Q"
            };
        }

        private static char GetAnioCodigo(int anio)
        {
            // Código de año según estándar VIN
            var year = anio % 30;
            return year switch
            {
                >= 1 and <= 9 => (char)('0' + year),
                >= 10 and <= 30 => (char)('A' + year - 10),
                _ => '1'
            };
        }

        private static char CalculateCheckDigit(string vin)
        {
            const string map = "0123456789.ABCDEFGH..JKLMN.P.R..STUVWXYZ";
            int[] weights = { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };

            int sum = 0;
            for (int i = 0; i < vin.Length; i++)
            {
                if (i == 8) continue; // Skip check digit position
                int value = map.IndexOf(vin[i]) % 10;
                sum += value * weights[i];
            }

            return (sum % 11 == 10) ? 'X' : (char)('0' + (sum % 11));
        }
    }
}