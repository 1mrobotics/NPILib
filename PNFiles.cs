namespace NPILib
{
    public class PNFiles
    {
        // Properties
        public string PartNumber { get; set; }
        public string XTRevision { get; set; }
        public string XTFullPath { get; set; }
        public string PDFRevision { get; set; }
        public string PDFFullPath { get; set; }

        // Constructor
        public PNFiles(string partNumber, string xtRevision, string xtFullPath, string pdfRevision, string pdfFullPath)
        {
            PartNumber = partNumber;
            XTRevision = xtRevision;
            XTFullPath = xtFullPath;
            PDFRevision = pdfRevision;
            PDFFullPath = pdfFullPath;
        }

        // Default Constructor (optional)
        public PNFiles()
        {
        }

        // ToString Override for Debugging (optional)
        public override string ToString()
        {
            return $"PartNumber: {PartNumber}, XTRevision: {XTRevision}, XTFullPath: {XTFullPath}, PDFRevision: {PDFRevision}, PDFFullPath: {PDFFullPath}";
        }
    }
}
