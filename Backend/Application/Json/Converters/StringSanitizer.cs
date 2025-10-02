namespace Application.Json.Converters
{
    public static class StringSanitizer
    {
        private static readonly string[] Placeholders = { "string", "String", "STRING", "texto", "placeholder" };

        /// <summary>
        /// Devuelve null si el valor es null/empty/whitespace o un placeholder (string/texto/placeholder).
        /// Sino, devuelve el texto con Trim.
        /// </summary>
        public static string? NormalizeOrNull(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            var trimmed = value.Trim();
            foreach (var p in Placeholders)
            {
                if (string.Equals(trimmed, p, StringComparison.OrdinalIgnoreCase))
                    return null;
            }
            return trimmed;
        }

        /// <summary>
        /// Igual que arriba pero devuelve string.Empty si resulta nulo, útil para campos requeridos.
        /// </summary>
        public static string NormalizeRequired(string? value)
            => NormalizeOrNull(value) ?? string.Empty;

        /// <summary>
        /// Verifica si el valor es un placeholder conocido
        /// </summary>
        public static bool IsPlaceholder(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            var trimmed = value.Trim();
            foreach (var p in Placeholders)
            {
                if (string.Equals(trimmed, p, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}