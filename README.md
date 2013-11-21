ExcelDnaDoc
===================
ExcelDnaDoc is a command-line utility to create a compiled HTML Help Workshop file (.chm)


Build Scripts
------------------
To build compiled help file (.chm) the HTML Help Workshop must be installed. A sample project can be found in the ExceDnaDocSample solution. To see an example verify the path of the HTML Help Compiler in buildSample.fsx. Then run the FAKE build script (buildSample.bat) using the command prompt.  

Usage
------------------
Usage: ExcelDnaDoc.exe dnaPath [/O outputPath] [/Y]  
  dnaPath      The path to the primary .dna file for the ExcelDna add-in.
  /Y           If the output folder exists, overwrite without prompting. (not implemented yet)  
  /O outPath   Output folder path - default is <dnaPath>/content. (not implemented yet)  

Example: ExcelDnaDoc.exe <build folder>\SampleLib-AddIn.dna  
         The HTML Help Workshop content will be created in <build folder>\content\.  

External libraries that have been marked as ExplicitExports=""true"" will be searched for UDFs that have been marked and documented using the ExcelFunctionAttribute and the ExcelArgumentAttribute.  

If The ExcelDna.Documentation library has been referenced then the ExcelFunctionSummaryAttribute is also available to include a longer function summary that will not be exposed in the Excel Function Wizard, but will be included in the HTML Help Workshop content.  

Dependencies
------------------
 NuGet Package Manager(http://nuget.codeplex.com/)  
 FAKE (F# MAKE) (http://fsharp.github.io/FAKE/)  
 Excel-DNA (http://exceldna.codeplex.com/)  
 RazorEngine(https://github.com/Antaris/RazorEngine)  
 HTML Help Workshop(http://msdn.microsoft.com/en-us/library/windows/desktop/ms669985(v=vs.85).aspx)  

