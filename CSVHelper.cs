namespace NPILib
{
    public static class CSVHelper
    {
        public static string EscapeCsvField(string field)
        {
            if (string.IsNullOrWhiteSpace(field)) return string.Empty;

            if (field.Contains(",") || field.Contains("\"") || field.Contains(" "))
            {
                field = $"\"{field.Replace("\"", "\"\"")}\"";
            }

            return field;
        }
    }
}
