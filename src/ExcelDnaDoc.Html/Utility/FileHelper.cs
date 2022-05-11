namespace ExcelDnaDoc.Utility
{
    using System;
    using System.IO;

    public static class FileHelper
    {
        public static void Move(string sourcePath, string destinationPath)
        {
            try
            {
                if (!File.Exists(sourcePath))

                    // Ensure that the source does exist.
                    if (File.Exists(sourcePath))
                        throw new FileNotFoundException("sourceFile not found", sourcePath);

                // Ensure that the target does not exist.
                if (File.Exists(destinationPath))
                    File.Delete(destinationPath);

                // Move the file.
                File.Move(sourcePath, destinationPath);
                Console.WriteLine("{0} was moved to {1}.", sourcePath, destinationPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}