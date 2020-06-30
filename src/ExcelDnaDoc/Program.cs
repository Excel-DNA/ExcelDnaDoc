namespace ExcelDnaDoc
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal class Program
    {
        private static string usageInfo =
@"ExcelDnaDoc Usage
------------------
ExcelDnaDoc is a command-line utility to create a compiled HTML Help Workshop file (.chm)

Usage: ExcelDnaDoc.exe dnaPath [/X]
  dnaPath                 The path to the primary .dna file for the ExcelDna add-in.
  /ExcludeHidden or /X    Optional. Excludes hidden functions from being documented if provided.
  /SkipCompile or /S      Optional. Skips compiling the HTML Help file (.chm) if provided.
  /Async or /A            Optional. Runs in async mode, consuming more cpu, but taking less time to run.

Example: ExcelDnaDoc.exe <build folder>\SampleLib-AddIn.dna
         The HTML Help Workshop content will be created in <build folder>\HelpContent\.

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
                Console.Write("dnaPath not provided.\r\n\r\n" + usageInfo);
#if DEBUG
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
#endif
                return;
            }

            string dnaPath = args[0];

            // verify dna file exists

            if (!File.Exists(dnaPath))
            {
                Console.Write("No dna file found at the specified location.");
#if DEBUG
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
#endif
            }
            else
            {
                bool excludeHidden = args.Any(
                    x => x.Equals(
                        @"/ExcludeHidden", StringComparison.OrdinalIgnoreCase) || 
                        x.Equals(@"/X", StringComparison.OrdinalIgnoreCase));

                bool skipCompile = args.Any(
                    x => x.Equals(
                        @"/SkipCompile", StringComparison.OrdinalIgnoreCase) || 
                        x.Equals(@"/S", StringComparison.OrdinalIgnoreCase));

                bool runAsync = args.Any(
                    x => x.Equals(
                        @"/Async", StringComparison.OrdinalIgnoreCase) ||
                        x.Equals(@"/A", StringComparison.OrdinalIgnoreCase));

                HtmlHelp.Create(dnaPath, excludeHidden: excludeHidden, skipCompile: skipCompile);
                Console.WriteLine("Successful");
#if DEBUG
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
#endif
            }
        }
    }
}