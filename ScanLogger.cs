using System;
using System.Collections.Generic;

namespace NPILib
{
    public static class ScanLogger
    {
        private static readonly List<string> SkippedFiles = new List<string>();

        public static void LogSkippedFile(string fileName)
        {
            SkippedFiles.Add(fileName);
        }

        public static void DisplaySkippedFiles()
        {
            Console.WriteLine("\nSkipped Files:");
            if (SkippedFiles.Count > 0)
            {
                foreach (var file in SkippedFiles)
                {
                    Console.WriteLine($"- {file}");
                }
            }
            else
            {
                Console.WriteLine("No files were skipped.");
            }
        }

        public static void ClearLogs()
        {
            SkippedFiles.Clear();
        }
    }
}
