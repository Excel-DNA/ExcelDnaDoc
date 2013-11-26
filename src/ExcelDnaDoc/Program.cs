namespace ExcelDnaDoc
{
    using System;
    using System.IO;
    using System.Reflection;

    internal class Program
    {
        private static string usageInfo =
@"ExcelDnaDoc Usage
------------------
ExcelDnaDoc is a command-line utility to create a compiled HTML Help Workshop file (.chm)

Usage: ExcelDnaDoc.exe dnaPath [/O outputPath] [/Y]
  dnaPath      The path to the primary .dna file for the ExcelDna add-in.

Example: ExcelDnaDoc.exe <build folder>\SampleLib-AddIn.dna
         The HTML Help Workshop content will be created in <build folder>\content\.

External libraries that have been marked as ExplicitExports=""true"" will be searched for
UDFs that have been marked and documented using the ExcelFunctionAttribute and the
ExcelArgumentAttribute.

If The ExcelDna.Documentation library has been referenced then the ExcelFunctionSummaryAttribute
is also available to include a longer function summary that will not be exposed in the Excel
Function Wizard, but will be included in the HTML Help Workshop content.
";

        private static void Main(string[] args)
        {
            ////TODO: Embed dependent dll's into ExcelDnaDoc.exe

            if (args.Length < 1)
            {
                Console.Write("No .dna file specified.\r\n\r\n" + usageInfo);
#if DEBUG
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
#endif
                return;
            }

            string dnaPath = args[0];
            string dnaDirectory = Path.GetDirectoryName(dnaPath);
            string dnaFilePrefix = Path.GetFileNameWithoutExtension(dnaPath);
            string helpProject = Path.Combine(Path.GetDirectoryName(dnaPath), string.Format("content/{0}.hhp", dnaFilePrefix));
            string sourceChm = Path.Combine(dnaDirectory, string.Format("content/{0}.chm", dnaFilePrefix));
            string destinationChm = Path.Combine(dnaDirectory, string.Format("{0}.chm", dnaFilePrefix));

            // create HTML Help content
            Console.WriteLine("creating HTML Help content");
            Console.WriteLine();
            ExcelDnaDoc.HtmlHelp.Create(dnaPath);

            // compile HTML Help
            Console.WriteLine("creating chm file");
            Utility.HtmlHelpWorkshopHelper.Compile(helpProject);
            Console.WriteLine();
            Console.WriteLine();

            // move HTML Help chm file to the main build folder
            Utility.FileHelper.Move(sourceChm, destinationChm);

            Console.WriteLine();
            Console.WriteLine("-- finished --");
#if DEBUG
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
#endif
        }
    }
}