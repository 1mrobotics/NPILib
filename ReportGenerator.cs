using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace NPILib
{
    public static class ReportGenerator
    {
        public static string GeneratePNFilesReport(List<PNFiles> pnFilesList, string outputDirectory)
        {
            Console.WriteLine("Starting PDF report generation...");

            ValidateInputs(pnFilesList, outputDirectory);

            if (!Directory.Exists(outputDirectory))
            {
                Console.WriteLine($"Creating output directory: {outputDirectory}");
                Directory.CreateDirectory(outputDirectory);
            }

            string timestamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm");
            string outputPath = Path.Combine(outputDirectory, $"PNFiles{timestamp}.pdf");

            if (Directory.Exists(outputPath))
                throw new IOException($"Cannot create file because a directory with the same name '{outputPath}' exists.");

            using (PdfDocument pdf = new PdfDocument())
            {
                pdf.Info.Title = "PNFiles Report";

                XFont titleFont = new XFont("Arial", 18, XFontStyle.Bold);
                XFont headerFont = new XFont("Arial", 12, XFontStyle.Bold);
                XFont regularFont = new XFont("Arial", 10, XFontStyle.Regular);
                double leftMargin = 40, rightMargin = 40, topMargin = 50, bottomMargin = 50;

                PdfPage page = pdf.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                double availableWidth = page.Width - leftMargin - rightMargin;
                double[] columnWidths = CalculateColumnWidths(pnFilesList, availableWidth);

                double y = DrawTitle(gfx,
                     new XFont("Arial", 20, XFontStyle.Bold), // Title font
                     new XFont("Arial", 10, XFontStyle.Regular), // Subtitle font
                     leftMargin, availableWidth, topMargin);


                DrawHeaders(gfx, headerFont, y, columnWidths);
                y += 20;

                int rowIndex = 1; // Start with row 1
                foreach (var pnFile in pnFilesList)
                {
                    if (y > page.Height - bottomMargin)
                    {
                        page = pdf.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = topMargin;
                        DrawHeaders(gfx, headerFont, y, columnWidths);
                        y += 20;
                    }

                    DrawRow(gfx, regularFont, y, columnWidths, pnFile, rowIndex++);
                    y += 20;
                }

                pdf.Save(outputPath);
                Console.WriteLine($"PDF report successfully created at: {outputPath}");
                return outputPath;
            }
        }

        private static void ValidateInputs(List<PNFiles> pnFilesList, string outputDirectory)
        {
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
        }

        

        private static double DrawTitle(XGraphics gfx, XFont titleFont, XFont subtitleFont, double leftMargin, double availableWidth, double y)
        {
            // Main title
            gfx.DrawString("PNFiles Report", titleFont, XBrushes.Black,
                new XRect(leftMargin, y, availableWidth, 40), XStringFormats.TopCenter);
            y += 40;

            // Timestamp as subtitle
            string timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            gfx.DrawString($"Generated on: {timestamp}", subtitleFont, XBrushes.Gray,
                new XRect(leftMargin, y, availableWidth, 20), XStringFormats.TopCenter);
            y += 20;

            // Separator line
            gfx.DrawLine(XPens.Black, leftMargin, y, leftMargin + availableWidth, y);
            y += 10;

            return y;
        }


        private static void DrawHeaders(XGraphics gfx, XFont font, double y, double[] columnWidths)
        {
            double x = 40; // Left margin
            double headerHeight = 25;

            // Draw header row background
            gfx.DrawRectangle(XBrushes.LightGray, new XRect(x, y, columnWidths[0] + columnWidths[1] + columnWidths[2] + columnWidths[3] + columnWidths[4], headerHeight));

            // Headers with row number column added
            string[] headers = { "Row", "Part Number", "XT Revision", "XT Path", "PDF Revision", "PDF Path" };

            for (int i = 0; i < headers.Length; i++)
            {
                gfx.DrawString(headers[i], font, XBrushes.Black, new XRect(x + 5, y + 5, columnWidths[i] - 10, headerHeight - 10), XStringFormats.TopLeft);
                x += columnWidths[i];
            }
        }

        private static void DrawRow(XGraphics gfx, XFont font, double y, double[] columnWidths, PNFiles pnFile, int rowIndex)
        {
            double x = 40; // Left margin
            double rowHeight = 20;

            // Draw borders for each cell
            for (int i = 0; i < columnWidths.Length; i++)
            {
                gfx.DrawRectangle(XPens.Black, new XRect(x, y, columnWidths[i], rowHeight));
                x += columnWidths[i];
            }

            // Reset x for content
            x = 40;

            // Render row content
            gfx.DrawString(rowIndex.ToString(), font, XBrushes.Black, new XRect(x + 5, y + 5, columnWidths[0] - 10, rowHeight - 10), XStringFormats.TopLeft);
            x += columnWidths[0];

            gfx.DrawString(pnFile.PartNumber, font, XBrushes.Black, new XRect(x + 5, y + 5, columnWidths[1] - 10, rowHeight - 10), XStringFormats.TopLeft);
            x += columnWidths[1];

            gfx.DrawString(pnFile.XTRevision, font, XBrushes.Black, new XRect(x + 5, y + 5, columnWidths[2] - 10, rowHeight - 10), XStringFormats.TopLeft);
            x += columnWidths[2];

            string truncatedXTPath = TruncatePath(pnFile.XTFullPath, (int)(columnWidths[3] / 6));
            gfx.DrawString(truncatedXTPath, font, XBrushes.Black, new XRect(x + 5, y + 5, columnWidths[3] - 10, rowHeight - 10), XStringFormats.TopLeft);
            x += columnWidths[3];

            gfx.DrawString(pnFile.PDFRevision, font, XBrushes.Black, new XRect(x + 5, y + 5, columnWidths[4] - 10, rowHeight - 10), XStringFormats.TopLeft);
            x += columnWidths[4];

            string truncatedPDFPath = TruncatePath(pnFile.PDFFullPath, (int)(columnWidths[5] / 6));
            gfx.DrawString(truncatedPDFPath, font, XBrushes.Black, new XRect(x + 5, y + 5, columnWidths[5] - 10, rowHeight - 10), XStringFormats.TopLeft);
        }

        private static double[] CalculateColumnWidths(List<PNFiles> pnFilesList, double availableWidth)
        {
            // Ensure Part Number column is wide enough to fit all part numbers
            double maxPartNumberWidth = pnFilesList.Max(p => MeasureTextWidth(p.PartNumber, new XFont("Arial", 10, XFontStyle.Regular)));
            double partNumberWidth = Math.Max(maxPartNumberWidth + 20, availableWidth * 0.2);

            // Allocate remaining space for other columns
            double remainingWidth = availableWidth - partNumberWidth - 50; // Account for Row column
            double otherColumnWidth = remainingWidth / 4;

            return new[] { 50, partNumberWidth, otherColumnWidth, otherColumnWidth, otherColumnWidth, otherColumnWidth };
        }

        private static double MeasureTextWidth(string text, XFont font)
        {
            using (var tempPdf = new PdfDocument())
            {
                var tempPage = tempPdf.AddPage();
                using (var gfx = XGraphics.FromPdfPage(tempPage))
                {
                    return gfx.MeasureString(text, font).Width;
                }
            }
        }


        private static string TruncatePath(string path, int maxLength)
        {
            if (path.Length <= maxLength) return path;
            int visibleLength = maxLength / 2 - 3; // Adjust for ellipsis
            return path.Substring(0, visibleLength) + "..." + path.Substring(path.Length - visibleLength);
        }


    }
}
