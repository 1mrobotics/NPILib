using System;
using System.Collections.Generic;
using System.IO;

namespace NPILib
{
    public static class CSVCreator
    {
        /// <summary>
        /// Creates a CSV file for a list of general Files with a timestamped filename.
        /// </summary>
        public static string CreateFilesCSV(Files filesInstance, string outputDirectory)
        {
            if (filesInstance == null)
                throw new ArgumentNullException(nameof(filesInstance));

            if (string.IsNullOrWhiteSpace(outputDirectory))
                throw new ArgumentException("Output directory cannot be null or empty.", nameof(outputDirectory));

            if (!Directory.Exists(outputDirectory))
                throw new DirectoryNotFoundException($"The directory '{outputDirectory}' does not exist.");

            string timestamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm");
            string outputPath = Path.Combine(outputDirectory, $"Files{timestamp}.csv");

            using (var writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("FileName,FullPath,Type,PartNumber,Revision,IsProductionFile");

                foreach (var file in filesInstance.FileList)
                {
                    if (!file.IsValid) continue; // Skip invalid files

                    writer.WriteLine($"{CSVHelper.EscapeCsvField(file.FileName)}," +
                                     $"{CSVHelper.EscapeCsvField(file.Path)}," +
                                     $"{CSVHelper.EscapeCsvField(file.Type)}," +
                                     $"{CSVHelper.EscapeCsvField(file.PartNumber)}," +
                                     $"{CSVHelper.EscapeCsvField(file.Rev)}," +
                                     $"{CSVHelper.EscapeCsvField(file.IsProductionFile.ToString())}");
                }
            }

            Console.WriteLine($"CSV file successfully created at: {outputPath}");
            return outputPath;
        }

        /// <summary>
        /// Creates a CSV file for a list of PNFiles with a timestamped filename.
        /// </summary>
        public static string CreatePNFilesCSV(List<PNFiles> pnFilesList, string outputDirectory)
        {
            if (pnFilesList == null)
                throw new ArgumentNullException(nameof(pnFilesList));

            if (string.IsNullOrWhiteSpace(outputDirectory))
                throw new ArgumentException("Output directory cannot be null or empty.", nameof(outputDirectory));

            if (!Directory.Exists(outputDirectory))
                throw new DirectoryNotFoundException($"The directory '{outputDirectory}' does not exist.");

            string timestamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm");
            string outputPath = Path.Combine(outputDirectory, $"PNFiles{timestamp}.csv");

            using (var writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("PartNumber,XTRevision,XTFullPath,PDFRevision,PDFFullPath");

                foreach (var pnFile in pnFilesList)
                {
                    writer.WriteLine($"{CSVHelper.EscapeCsvField(pnFile.PartNumber)}," +
                                     $"{CSVHelper.EscapeCsvField(pnFile.XTRevision)}," +
                                     $"{CSVHelper.EscapeCsvField(pnFile.XTFullPath)}," +
                                     $"{CSVHelper.EscapeCsvField(pnFile.PDFRevision)}," +
                                     $"{CSVHelper.EscapeCsvField(pnFile.PDFFullPath)}");
                }
            }

            Console.WriteLine($"CSV file for PNFiles successfully created at: {outputPath}");
            return outputPath;
        }
    }
}
