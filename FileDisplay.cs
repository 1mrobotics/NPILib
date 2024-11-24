using System;
using System.Collections.Generic;

namespace NPILib
{
    public static class FileDisplay
    {
        public static void DisplayFiles(IEnumerable<ProdFile> fileList)
        {
            if (fileList == null)
            {
                Console.WriteLine("No files found.");
                return;
            }

            foreach (var file in fileList)
            {
                Console.WriteLine(file.GetFileInfo());
            }
        }
    }
}
