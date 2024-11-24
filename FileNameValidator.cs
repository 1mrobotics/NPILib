using System;
using System.IO;
using System.Linq;

namespace NPILib
{
    public static class FileNameValidator
    {
        public static bool IsValidFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            // Check for invalid characters
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (fileName.Any(c => invalidChars.Contains(c))) return false;

            // Additional rules: ensure no spaces or special patterns if required
            if (fileName.Contains(" ")) return false;

            return true;
        }
    }
}
