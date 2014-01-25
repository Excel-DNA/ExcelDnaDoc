ExcelDnaDoc - Documentation Builder for Excel-DNA
=================================================
ExcelDnaDoc is a command-line utility to create a compiled HTML Help Workshop file (.chm) for ExcelDna.

* single help file created even if multiple library are specified in the "dna" file.  
* can use customized templates and content  

Use the [issues log][issues] to report any issues or give feedback for future enhancements.

<div class="row">
  <div class="span1"></div>
  <div class="span6">
    <div class="well well-small" id="nuget">
      The ExcelDnaDoc library can be <a href="https://nuget.org/packages/ExcelDnaDoc">installed from NuGet</a>:
      <pre>PM> Install-Package ExcelDnaDoc</pre>
    </div>
  </div>
  <div class="span1"></div>
</div>

<p>Alternatively, you can download the <a href="https://github.com/mndrake/ExcelDnaDoc/zipball/master">source as a ZIP file</a> or download 
the <a href="https://github.com/mndrake/ExcelDnaDoc/zipball/release">compiled binaries</a> as a ZIP.</p>  

To build a compiled help file (.chm) the [HTML Help Workshop](http://msdn.microsoft.com/en-us/library/windows/desktop/ms669985(v=vs.85).aspx) (HHW) must be installed.
ExcelDnaDoc expects HHW to be installed at `C:\Program Files (x86)\HTML Help Workshop\`. 
If it is installed at another location change `packages/ExcelDnaDoc/tools/ExcelDnaDoc.exe.config` 
to reference the proper directory before compiling your project.  

When installed from NuGet it will edit the default .dna file installed by Excel-DNA and adds post 
build steps to build the .chm documentation file whenever the project is build.

Notes
------------------

Uses the `ExcelFunction`, `ExcelArgument`, and `ExcelCommand` attributes in Excel-DNA to build 
documentation for your Excel-DNA add-in.  

The following fields are can be used to create documentation :  

### _ExcelFunction Attribute_
|					|																			|
| ----------------- | ------------------------------------------------------------------------- |
| `Name`			| if not given the actual method name is used								|
| `Description`		| if not used no description will be included in documentation				|
| `Category`		| if not given functions will be grouped under "*<project name>* Functions" | 
| `HelpTopic`		| can be used to link function to generated help in Excel's function wizard | 

### _ExcelArgument Attribute_
|					|																			|
| ----------------- | ------------------------------------------------------------------------- |
| `Name`			| if not given the actual parameter name is used							|
| `Description`		| if not used no description will be included in documentation				|

### _ExcelCommand Attribute_  
|					|																			|
| ----------------- | ------------------------------------------------------------------------- |
| `Name`			| if not given the actual parameter name is used							|
| `Description`		| if not used no description will be included in documentation				|
| `HelpTopic`		| can be used to link function to generated help in Excel's function wizard |
| `ShortCut`		| if not used no shortcut will be included in documentation					|

If ExcelDna.Documentation is included as a reference (default in NuGet package) then an additional 
attribute `ExcelFunctionDoc` is available as a replacement to the `ExcelFunction` attribute 
which includes additional fields that can be used for additional documentation.

### _ExcelFunctionDoc Attribute_
|					|																			|
| ----------------- | ------------------------------------------------------------------------- |
| `Name`			| if not given the actual method name is used								|
| `Description`		| if not used no description will be included in documentation				|
| `Category`		| if not given functions will be grouped under "*<project name>* Functions" | 
| `HelpTopic`		| can be used to link function to generated help in Excel's function wizard | 
| `Returns`			| description of the return value											|
| `Summary`			| longer discussion of function included in documentation					|  
| `Remarks`			| remarks on usage and / or possible errors									|


Example
------------------

**F#**  

    [lang=fsharp]
    namespace DocTest
    open ExcelDna.Integration
    open ExcelDna.Documentation

    module Math =

        [<ExcelFunctionDoc( Name = "Math.AddThem", Category = "Math", 
                            Description = "adds two numbers", 
                            HelpTopic = "DocTest-AddIn.chm!1001",
                            Summary = "really all it does is add two number ... I promise.",
                            Returns = "the sum of the two arguments")>]
        let addThem
            (
                [<ExcelArgument(Name = "Arg1", Description = "the first argument")>]a,
                [<ExcelArgument(Name = "Arg2", Description = "the second argument")>]b
            ) = 
        
            a+b

**C#**  

    [lang=csharp]
    namespace DocTest
    {
        using ExcelDna.Integration;
    
        public class Text 
        {
            [ExcelFunction( Name = "Text.ConcatThem", 
                            Description = "concatenates two strings", 
                            HelpTopic = "DocTest-AddIn.chm!1002")]
            public static object ConcatThem(
                [ExcelArgument(Description="the first string")] object a, 
                [ExcelArgument(Description="the second string")] object b)
            {
                return string.Concat(a.ToString(), b.ToString());
            }
        }
    }

Command Line Usage
------------------
    ExcelDnaDoc.exe dnaPath  
`dnaPath` The path to the primary .dna file for the ExcelDna add-in.  

Example: `ExcelDnaDoc.exe <build folder>\SampleLib-AddIn.dna`  
         The HTML Help Workshop content will be created in `<build folder>\HelpContent\`.  

External libraries will be searched for UDFs and Commands
that are exposed to Excel and documented using the ExcelFunctionAttribute and the ExcelArgumentAttribute.  

If The ExcelDna.Documentation library has been referenced then the ExcelFunctionDocAttribute 
is also available to include additional documentation fields that will not be exposed in the Excel Function 
Wizard, but will be included in the HTML Help Workshop content.  

This Project Uses
-----------------
 [Excel-DNA](http://exceldna.codeplex.com/)  
 [FAKE (F# MAKE)](http://fsharp.github.io/FAKE/)  
 [HTML Help Workshop ](http://msdn.microsoft.com/en-us/library/windows/desktop/ms669985(v=vs.85).aspx)  
 [NuGet Package Manager](http://nuget.codeplex.com/)  
 [RazorEngine](https://github.com/Antaris/RazorEngine)  
 
Contributing and copyright
--------------------------

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork 
the project and submit pull requests.  

The library is available under the MIT license, which allows modification and 
redistribution for both commercial and non-commercial purposes. For more information see the 
[License file][license] in the GitHub repository. 

  [content]: https://github.com/mndrake/ExcelDnaDoc/tree/master/docs/content
  [gh]: https://github.com/mndrake/ExcelDnaDoc
  [issues]: https://github.com/mndrake/ExcelDnaDoc/issues
  [readme]: https://github.com/mndrake/ExcelDnaDoc/blob/master/README.md
  [license]: https://github.com/mndrake/ExcelDnaDoc/blob/master/LICENSE.txt
