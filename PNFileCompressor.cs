using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace NPILib
{
    public static class PNFileCompressor
    {
        public static void CompressFiles(List<PNFiles> pnFilesList, string outputZipPath)
        {
            if (pnFilesList == null || pnFilesList.Count == 0)
                throw new ArgumentException("The list of PNFiles cannot be null or empty.", nameof(pnFilesList));

            if (string.IsNullOrWhiteSpace(outputZipPath))
                throw new ArgumentException("The output ZIP path cannot be null or empty.", nameof(outputZipPath));

            string outputDirectory = Path.GetDirectoryName(outputZipPath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Check if the file already exists
            if (System.IO.File.Exists(outputZipPath))
            {
                // Prompt the user or decide automatically (here we choose to delete the file)
                System.IO.File.Delete(outputZipPath);
            }

            using (var zipArchive = ZipFile.Open(outputZipPath, ZipArchiveMode.Create))
            {
                foreach (var pnFile in pnFilesList)
                {
                    AddFileToZip(zipArchive, pnFile.XTFullPath);
                    AddFileToZip(zipArchive, pnFile.PDFFullPath);
                }
            }
        }


        private static void AddFileToZip(ZipArchive zipArchive, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || filePath == "N/A" || !System.IO.File.Exists(filePath))
                return;

            string fileName = Path.GetFileName(filePath);
            zipArchive.CreateEntryFromFile(filePath, fileName, CompressionLevel.Optimal);
        }
    }
}
