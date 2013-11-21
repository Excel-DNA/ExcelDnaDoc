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
  /Y           If the output folder exists, overwrite without prompting. (not implemented yet)
  /O outPath   Output folder path - default is <dnaPath>/content. (not implemented yet)

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
            ////AppDomain.CurrentDomain.AssemblyResolve += (sender, arg) =>
            ////{
            ////    Assembly thisAssembly = Assembly.GetExecutingAssembly();

            ////    //Get the Name of the AssemblyFile
            ////    var name = arg.Name.Substring(0, arg.Name.IndexOf(',')) + ".dll";

            ////    //Load form Embedded Resources - This Function is not called if the Assembly is in the Application Folder
            ////    var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name));
            ////    if (resources.Count() > 0)
            ////    {
            ////        var resourceName = resources.First();
            ////        using (Stream stream = thisAssembly.GetManifestResourceStream(resourceName))
            ////        {
            ////            if (stream == null) return null;
            ////            var block = new byte[stream.Length];
            ////            stream.Read(block, 0, block.Length);
            ////            return Assembly.Load(block);
            ////        }
            ////    }
            ////    return null;
            ////};

            ////string resource1 = "ExcelDnaDoc.Resources.ExcelDna.Integration.dll";
            ////EmbeddedAssembly.Load(resource1, "ExcelDna.Integration.dll");
            ////string resource2 = "ExcelDnaDoc.Resources.System.Web.Razor.dll";
            ////EmbeddedAssembly.Load(resource2, "System.Web.Razor.dll");
            ////string resource3 = "ExcelDnaDoc.Resources.RazorEngine.dll";
            ////EmbeddedAssembly.Load(resource3, "RazorEngine.dll");

            ////AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

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

            // create HTML Help content
            Console.WriteLine("creating HTML Help content");
            ExcelDnaDoc.HtmlHelp.Create(dnaPath);

            //// TODO: reference HTML Help Compiler instead of using FAKE build script
            //// compiling HTML Help content
            //// Console.WriteLine("creating chm file");

            Console.WriteLine("-- finished --");
#if DEBUG
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
#endif
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }
    }
}