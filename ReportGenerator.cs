using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Collections.Generic;
using System.IO;
using System;

namespace NPILib
{
    public static class ReportGenerator
    {
        public static string GeneratePNFilesReport(List<PNFiles> pnFilesList, string outputDirectory)
        {
            Console.WriteLine("Starting PDF report generation...");

            // Validate inputs
            if (pnFilesList == null || pnFilesList.Count == 0)
                throw new ArgumentException("PNFiles list cannot be null or empty.", nameof(pnFilesList));

            if (string.IsNullOrWhiteSpace(outputDirectory))
                throw new ArgumentException("Output directory cannot be null or empty.", nameof(outputDirectory));

            foreach (var pnFile in pnFilesList)
            {
                if (string.IsNullOrWhiteSpace(pnFile.PartNumber) ||
                    string.IsNullOrWhiteSpace(pnFile.XTFullPath) ||
                    string.IsNullOrWhiteSpace(pnFile.PDFFullPath))
                {
                    throw new InvalidOperationException("PNFiles list contains invalid data.");
                }
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Console.WriteLine($"Creating output directory: {outputDirectory}");
                Directory.CreateDirectory(outputDirectory);
            }

            // Generate dynamic file name with timestamp
            string timestamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm");
            string outputPath = Path.Combine(outputDirectory, $"PNFiles{timestamp}.pdf");

            try
            {
                // Check for conflicting directory names
                if (Directory.Exists(outputPath))
                    throw new IOException($"Cannot create file because a directory with the same name '{outputPath}' exists.");

                // Create a new PDF document
                PdfDocument pdf = new PdfDocument();
                pdf.Info.Title = "PNFiles Report";

                // Create a page
                PdfPage page = pdf.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 18, XFontStyle.Bold);
                XFont headerFont = new XFont("Arial", 12, XFontStyle.Bold);
                XFont regularFont = new XFont("Arial", 10, XFontStyle.Regular);

                double leftMargin = 40;
                double rightMargin = 40;
                double topMargin = 50;
                double bottomMargin = 50;
                double y = topMargin;

                // Define column widths (fixed)
                double availableWidth = page.Width - leftMargin - rightMargin;
                double[] columnWidths = {
                    availableWidth * 0.15, // Part Number
                    availableWidth * 0.1,  // XT Revision
                    availableWidth * 0.3,  // XT Path
                    availableWidth * 0.1,  // PDF Revision
                    availableWidth * 0.35  // PDF Path
                };

                // Add title
                gfx.DrawString("PNFiles Report", titleFont, XBrushes.Black,
                    new XRect(leftMargin, y, availableWidth, 30), XStringFormats.TopCenter);
                y += 40;

                // Add table headers
                AddRow(gfx, headerFont, y, columnWidths, new[] {
                    "Part Number", "XT Revision", "XT Path", "PDF Revision", "PDF Path"
                }, isHeader: true);
                y += 20;

                // Add table rows
                foreach (var pnFile in pnFilesList)
                {
                    if (y > page.Height - bottomMargin) // Start new page if out of space
                    {
                        page = pdf.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = topMargin;
                        AddRow(gfx, headerFont, y, columnWidths, new[] {
                            "Part Number", "XT Revision", "XT Path", "PDF Revision", "PDF Path"
                        }, isHeader: true);
                        y += 20;
                    }

                    AddRow(gfx, regularFont, y, columnWidths, new[] {
                        pnFile.PartNumber,
                        pnFile.XTRevision,
                        TruncateText(pnFile.XTFullPath, 50),
                        pnFile.PDFRevision,
                        TruncateText(pnFile.PDFFullPath, 50)
                    }, isHeader: false);
                    y += 20;
                }

                // Save the document
                pdf.Save(outputPath);
                pdf.Close();

                Console.WriteLine($"PDF report successfully created at: {outputPath}");
                return outputPath;
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"File I/O error: {ioEx.Message}\nStack Trace:\n{ioEx.StackTrace}");
                throw;
            }
        }

        private static void AddRow(XGraphics gfx, XFont font, double y, double[] columnWidths, string[] values, bool isHeader)
        {
            double x = 40; // Start from left margin
            XBrush brush = isHeader ? XBrushes.Black : XBrushes.Black;

            for (int i = 0; i < values.Length; i++)
            {
                gfx.DrawString(values[i], font, brush, new XRect(x, y, columnWidths[i], 20), XStringFormats.TopLeft);
                x += columnWidths[i]; // Move to the next column
            }
        }

        private static string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length > maxLength ? text.Substring(0, maxLength - 3) + "..." : text;
        }
    }
}
