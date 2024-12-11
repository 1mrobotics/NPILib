using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPILib
{
    public static class FileProcessor
    {
        public static void PerformAction(string filePath)
        {
            try
            {
                // Get the parent directory of the monitored folder
                string monitoredFolder = Path.GetDirectoryName(filePath);
                string parentFolder = Directory.GetParent(monitoredFolder)?.FullName;

                if (parentFolder == null)
                {
                    throw new InvalidOperationException("Parent folder could not be determined.");
                }

                // Define the PRFiles folder in the parent directory
                string prFolderPath = Path.Combine(parentFolder, "PRFiles");

                // Ensure the PRFiles folder exists
                if (!Directory.Exists(prFolderPath))
                {
                    Directory.CreateDirectory(prFolderPath);
                }

                // Generate the new file name with date and time
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string newFileName = Path.Combine(prFolderPath, $"{timestamp}.txt");

                // Create the new text file in the PRFiles folder
                File.WriteAllText(newFileName, "File created by NPILib");

                // Optional: Log a success message (for debugging purposes)
                File.AppendAllText(Path.Combine(prFolderPath, "NPILibLog.txt"),
                    $"{DateTime.Now}: Created file '{newFileName}'\n");
            }
            catch (Exception ex)
            {
                // Log errors to a library-specific log file in the PRFiles folder
                string logPath = Path.Combine(
                    Path.GetDirectoryName(filePath),
                    "PRFiles",
                    "NPILibErrorLog.txt"
                );
                File.AppendAllText(logPath, $"{DateTime.Now}: {ex}\n");
            }
        }
    }
}