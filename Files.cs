using System.Collections.Generic;

namespace NPILib
{
    public class Files
    {
        public List<ProdFile> FileList { get; private set; }

        public Files()
        {
            FileList = new List<ProdFile>();
        }

        // Add scanned files to the list
        public void AddFiles(IEnumerable<ProdFile> files)
        {
            FileList.AddRange(files);
        }
    }
}
