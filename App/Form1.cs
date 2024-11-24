using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NPILib;
using XpandNPIManager.Helpers;

namespace XpandNPIManager
{
    public partial class Form1 : Form
    {
        private readonly string logDirectoryBase = @"\\PDM\ERP-Data\Files\Logs";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreateZip_Click(object sender, EventArgs e)
        {
            try
            {
                // Step 1: Prompt the user to paste a list of part numbers
                string partNumbersInput = PromptHelper.PromptForPartNumbers();
                if (string.IsNullOrWhiteSpace(partNumbersInput))
                {
                    MessageBox.Show("No part numbers provided. Operation canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<string> partNumbers = new List<string>(partNumbersInput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));

                // Step 2: Ensure log directories exist
                string filesLogDir = Path.Combine(logDirectoryBase, "Files");
                string pnFilesLogDir = Path.Combine(logDirectoryBase, "PNFiles");
                EnsureDirectoryExists(filesLogDir);
                EnsureDirectoryExists(pnFilesLogDir);

                // Step 3: Scan production files
                string scanPath = @"\\PDM\ERP-Data\Files";
                if (!Directory.Exists(scanPath))
                {
                    MessageBox.Show($"The directory '{scanPath}' does not exist. Operation canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<ProdFile> productionFiles = ScanFiles(scanPath);

                // Step 4: Log production files list
                Files filesInstance = new Files { FileList = productionFiles };
                string filesLogPath = CSVCreator.CreateFilesCSV(filesInstance, filesLogDir);

                // Step 5: Create PNFiles using part numbers and production files
                List<PNFiles> pnFilesList = PNFileProcessor.GetPNFilesForPartNumbers(productionFiles, partNumbers);

                // Step 6: Log PN files list
                string pnFilesLogPath = CSVCreator.CreatePNFilesCSV(pnFilesList, pnFilesLogDir);

                // Step 7: Prompt user for ZIP folder location
                string outputZipPath = PromptHelper.PromptForOutputZipLocation();
                if (string.IsNullOrWhiteSpace(outputZipPath))
                {
                    MessageBox.Show("No output location selected. Operation canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Step 8: Create ZIP folder
                PNFileCompressor.CompressFiles(pnFilesList, outputZipPath);

                // Step 9: Notify user of success
                MessageBox.Show($"ZIP file created successfully.\n\nProduction Files CSV: {filesLogPath}\nPN Files CSV: {pnFilesLogPath}\nZIP: {outputZipPath}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<ProdFile> ScanFiles(string rootDirectory)
        {
            var files = new List<ProdFile>();

            foreach (string filePath in Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories))
            {
                try
                {
                    files.Add(new ProdFile(filePath)); // Updated to use ProdFile
                }
                catch
                {
                    // Skip invalid files
                }
            }
            return files;
        }

        private void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialization logic if needed
        }
    }
}
