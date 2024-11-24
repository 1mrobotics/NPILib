using System.IO;

namespace NPILib
{
    public static class CSVCreator
    {
        public static void CreateFilesCSV(string outputPath, Files filesInstance)
        {
            if (filesInstance == null)
                throw new System.ArgumentNullException(nameof(filesInstance));

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new System.ArgumentException("Output path cannot be null or empty.", nameof(outputPath));

            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                throw new DirectoryNotFoundException($"The directory '{Path.GetDirectoryName(outputPath)}' does not exist.");

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

            System.Console.WriteLine($"CSV file successfully created at: {outputPath}");
        }
    }
}
