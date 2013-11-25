ExcelDnaDoc
===================
ExcelDnaDoc is a command-line utility to create a compiled HTML Help Workshop file (.chm) for ExcelDna.


NuGet Package
------------------
https://www.nuget.org/packages/ExcelDnaDoc/

To build compiled help file (.chm) the HTML Help Workshop (HHW) must be installed (http://msdn.microsoft.com/en-us/library/windows/desktop/ms669985(v=vs.85).aspx).
ExcelDnaDoc expects HHW to be installed at `C:\Program Files (x86)\HTML Help Workshop\`. If it is installed at another location change `packages/ExcelDnaDoc/tools/ExcelDnaDoc.exe.config` to
reference the proper directory before compiling your project.

*ExcelDnaDoc.exe.config*
    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
      <appSettings>
        <add key="HtmlHelpWorkshopCompilerPath" value="C:\Program Files (x86)\HTML Help Workshop\hhc.exe"/>
      </appSettings>
    </configuration>

When installed from NuGet it will replace the default .dna file installed by Excel-DNA and adds post build steps to build the .chm documentation file whenever
the project is build.

Example
------------------

*F#*

    open ExcelDna.Integration
    open ExcelDna.Documentation

    module Math =

        [<ExcelFunction( Name = "Math.AddThem", Description = "adds two numbers", 
                         HelpTopic="DocTest-AddIn.chm!1001")>]
        [<ExcelFunctionSummary("really all it does is add two number ... I promise.")>]
        let addThem
            (
                [<ExcelArgument(Name = "Arg1", Description = "the first number")>]a,
                [<ExcelArgument(Name = "Arg2", Description = "the second number")>]b
            ) = 
            
            a+b

*C#*

    public class Text 
    {
        [ExcelFunction( Name = "Text.ConcatThem", Description = "concatenates two strings", 
                        HelpTopic = "DocTest-AddIn.chm!1002")]
        public static object ConcatThem(
            [ExcelArgument(Description="the first string")] object a, 
            [ExcelArgument(Description="the second string")] object b)
        {
            return string.Concat(a.ToString(), b.ToString());
        }
    }

Usage
------------------
    ExcelDnaDoc.exe dnaPath  
`dnaPath` The path to the primary .dna file for the ExcelDna add-in.  

Example: `ExcelDnaDoc.exe <build folder>\SampleLib-AddIn.dna`  
         The HTML Help Workshop content will be created in `<build folder>\content\`.  

External libraries that have been marked as ExplicitExports="true" will be searched for UDFs that have been marked and documented using the ExcelFunctionAttribute and the ExcelArgumentAttribute.  

If The ExcelDna.Documentation library has been referenced then the ExcelFunctionSummaryAttribute is also available to include a longer function summary that will not be exposed in the Excel Function Wizard, but will be included in the HTML Help Workshop content.  

Dependencies
------------------
 NuGet Package Manager(http://nuget.codeplex.com/)  
 FAKE (F# MAKE) (http://fsharp.github.io/FAKE/)  
 Excel-DNA (http://exceldna.codeplex.com/)  
 RazorEngine(https://github.com/Antaris/RazorEngine)  
 HTML Help Workshop(http://msdn.microsoft.com/en-us/library/windows/desktop/ms669985(v=vs.85).aspx)  

[![githalytics.com alpha](https://cruel-carlota.pagodabox.com/6d1b41edf1ed32e109771bb99bbe87bd "githalytics.com")](http://githalytics.com/mndrake/ExcelDnaDoc)
