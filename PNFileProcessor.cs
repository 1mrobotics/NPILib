using System;
using System.Collections.Generic;
using System.Linq;


namespace NPILib
{
    public static class PNFileProcessor
    {
        /// <summary>
        /// Processes a list of files to populate a PNFiles instance for a single part number.
        /// </summary>
        public static PNFiles GetPNFiles(List<ProdFile> files, string partNumber)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("File list cannot be null or empty.", nameof(files));

            if (string.IsNullOrWhiteSpace(partNumber))
                throw new ArgumentException("Part number cannot be null or empty.", nameof(partNumber));

            var matchingFiles = files.Where(f => f.PartNumber == partNumber && f.IsProductionFile).ToList();

            if (!matchingFiles.Any())
                return new PNFiles(partNumber, "N/A", "N/A", "N/A", "N/A");

            var xtFile = matchingFiles
                .Where(f => f.Type == "x_t")
                .OrderByDescending(f => f.Rev)
                .FirstOrDefault();

            var pdfFile = matchingFiles
                .Where(f => f.Type == "pdf")
                .OrderByDescending(f => f.Rev)
                .FirstOrDefault();

            return new PNFiles(
                partNumber,
                xtFile?.Rev ?? "N/A",
                xtFile?.Path ?? "N/A",
                pdfFile?.Rev ?? "N/A",
                pdfFile?.Path ?? "N/A"
            );
        }

        /// <summary>
        /// Processes a list of part numbers and files to return a list of PNFiles instances.
        /// </summary>
        /// <param name="files">List of File instances.</param>
        /// <param name="partNumbers">List of part numbers to process.</param>
        /// <returns>List of PNFiles instances populated for each part number.</returns>
        public static List<PNFiles> GetPNFilesForPartNumbers(List<ProdFile> files, List<string> partNumbers)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("File list cannot be null or empty.", nameof(files));

            if (partNumbers == null || partNumbers.Count == 0)
                throw new ArgumentException("Part number list cannot be null or empty.", nameof(partNumbers));

            var pnFilesList = new List<PNFiles>();

            foreach (var partNumber in partNumbers)
            {
                var pnFiles = GetPNFiles(files, partNumber);
                pnFilesList.Add(pnFiles);
            }

            return pnFilesList;
        }
    }
}
