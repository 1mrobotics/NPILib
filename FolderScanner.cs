using System.Collections.Generic;
using System.IO;
using System;



namespace NPILib
{
    public static class FolderScanner
    {
        public static List<ProdFile> ScanFolder(string folderPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folderPath))
                    throw new ArgumentException("Folder path cannot be null or empty.", nameof(folderPath));

                if (!Directory.Exists(folderPath))
                    throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist or is inaccessible.");

                var fileList = new List<ProdFile>();
                var files = Directory.GetFiles(folderPath);

                foreach (var filePath in files)
                {
                    var file = new ProdFile(filePath);

                    if (!file.IsValid)
                    {
                        ScanLogger.LogSkippedFile(file.FileName);
                        continue;
                    }

                    fileList.Add(file);
                }

                return fileList;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied to folder '{folderPath}': {ex.Message}");
                return new List<ProdFile>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scanning folder '{folderPath}': {ex.Message}");
                return new List<ProdFile>();
            }
        }


        public static List<ProdFile> ScanFolderRecursively(string folderPath)
        {
            var fileList = new List<ProdFile>();
            fileList.AddRange(ScanFolder(folderPath));

            var subdirectories = Directory.GetDirectories(folderPath);
            foreach (var subdirectory in subdirectories)
            {
                fileList.AddRange(ScanFolderRecursively(subdirectory));
            }

            return fileList;
        }
    }
}
