using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace NPILib
{
    public static class ReportGenerator
    {
        public static void GeneratePNFilesReport(List<PNFiles> pnFilesList, string outputPath)
        {
            if (pnFilesList == null || pnFilesList.Count == 0)
                throw new ArgumentException("PNFiles list cannot be null or empty.", nameof(pnFilesList));

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Output path cannot be null or empty.", nameof(outputPath));

            string directory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Create a PDF writer instance
            using (PdfWriter writer = new PdfWriter(outputPath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);

                    // Add title
                    document.Add(new Paragraph("PNFiles Report")
                        .SetFontSize(18)
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(20));

                    // Create a table with 5 columns
                    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 3, 1, 3 }))
                        .UseAllAvailableWidth()
                        .SetMarginBottom(20);

                    // Add table headers
                    AddCell(table, "Part Number", true);
                    AddCell(table, "XT Revision", true);
                    AddCell(table, "XT Path", true);
                    AddCell(table, "PDF Revision", true);
                    AddCell(table, "PDF Path", true);

                    // Add data rows
                    foreach (var pnFile in pnFilesList)
                    {
                        AddCell(table, pnFile.PartNumber);
                        AddCell(table, pnFile.XTRevision);
                        AddCell(table, TruncateText(pnFile.XTFullPath, 40));
                        AddCell(table, pnFile.PDFRevision);
                        AddCell(table, TruncateText(pnFile.PDFFullPath, 40));
                    }

                    // Add the table to the document
                    document.Add(table);

                    // Add footer
                    document.Add(new Paragraph($"Generated on: {DateTime.Now}")
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.RIGHT));
                }
            }

            Console.WriteLine($"PDF report successfully created at: {outputPath}");
        }

        private static void AddCell(Table table, string content, bool isHeader = false)
        {
            Cell cell = new Cell().Add(new Paragraph(content));
            if (isHeader)
            {
                cell.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER);
            }
            else
            {
                cell.SetTextAlignment(TextAlignment.LEFT);
            }
            table.AddCell(cell);
        }

        private static string TruncateText(string text, int maxLength)
        {
            return string.IsNullOrEmpty(text) ? text : text.Length > maxLength ? text.Substring(0, maxLength - 3) + "..." : text;
        }
    }
}
