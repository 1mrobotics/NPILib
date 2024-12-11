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
            string networkPath = @"\\PDM\ERP-Data\Files";

            try
            {
                // Step 1: Check connection to the network path
                if (!ConnectionManager.TestConnection(networkPath))
                {
                    // Step 2: Prompt for credentials if connection fails
                    var credentials = UserPromptHelper.PromptForCredentials();
                    if (credentials == null)
                    {
                        MessageBox.Show("No credentials provided. Operation canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Step 3: Attempt to connect with the provided credentials
                    if (!ConnectionManager.ConnectToNetwork(networkPath, credentials.UserName, credentials.Password))
                    {
                        MessageBox.Show("Failed to connect to the network path. Please check your credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Step 4: Prompt the user to paste a list of part numbers
                string partNumbersInput = PromptHelper.PromptForPartNumbers();
                if (string.IsNullOrWhiteSpace(partNumbersInput))
                {
                    MessageBox.Show("No part numbers provided. Operation canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<string> partNumbers = new List<string>(partNumbersInput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));

                // Step 5: Ensure log directories exist
                string filesLogDir = Path.Combine(logDirectoryBase, "Files");
                string pnFilesLogDir = Path.Combine(logDirectoryBase, "PNFiles");
                EnsureDirectoryExists(filesLogDir);
                EnsureDirectoryExists(pnFilesLogDir);

                // Step 6: Scan production files
                if (!Directory.Exists(networkPath))
                {
                    MessageBox.Show($"The directory '{networkPath}' does not exist. Operation canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<ProdFile> productionFiles = ScanFiles(networkPath);

                // Step 7: Log production files list
                Files filesInstance = new Files { FileList = productionFiles };
                string filesLogPath = CSVCreator.CreateFilesCSV(filesInstance, filesLogDir);

                // Step 8: Create PNFiles using part numbers and production files
                List<PNFiles> pnFilesList = PNFileProcessor.GetPNFilesForPartNumbers(productionFiles, partNumbers);

                // Step 9: Log PN files list
                string pnFilesLogPath = CSVCreator.CreatePNFilesCSV(pnFilesList, pnFilesLogDir);

                // Step 10: Generate PDF Report (no hardcoded name here)
                string pdfReportPath;
                try
                {
                    pdfReportPath = ReportGenerator.GeneratePNFilesReport(pnFilesList, pnFilesLogDir);
                }
                catch (Exception pdfEx)
                {
                    // Log the error and continue with the flow
                    MessageBox.Show($"Failed to create PDF report: {pdfEx.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pdfReportPath = "Failed to generate PDF report.";
                }

                // Step 11: Prompt user for ZIP folder location
                string outputZipPath = PromptHelper.PromptForOutputZipLocation();
                if (string.IsNullOrWhiteSpace(outputZipPath))
                {
                    MessageBox.Show("No output location selected. Operation canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Step 12: Create ZIP folder
                PNFileCompressor.CompressFiles(pnFilesList, outputZipPath);

                // Step 13: Notify user of success
                MessageBox.Show($"ZIP file created successfully.\n\nProduction Files CSV: {filesLogPath}\nPN Files CSV: {pnFilesLogPath}\nPDF Report: {pdfReportPath}\nZIP: {outputZipPath}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Always disconnect from the network path
                ConnectionManager.DisconnectNetwork(networkPath);
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
