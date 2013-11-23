// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------

#r "packages/FAKE/tools/FakeLib.dll"

open System
open System.IO
open Fake 
open Fake.AssemblyInfoFile

// --------------------------------------------------------------------------------------
// Information about the project to be used at NuGet and in AssemblyInfo files
// --------------------------------------------------------------------------------------

let project = "ExcelDnaDoc"
let authors = ["David Carlson"]
let summary = "command-line utility to create a compiled HTML Help Workshop file (.chm) for ExcelDna"
let description = """
  To build compiled help file (.chm) the HTML Help Workshop must be installed. A
  sample project can be found in the ExceDnaDocSample solution. To see an example
  verify the path of the HTML Help Compiler in buildSample.fsx and then run the
  FAKE build script (buildSample.bat) using the command prompt."""
let tags = "Excel-DNA"

let gitHome = "https://github.com/mndrake"
let gitName = "ExcelDnaDoc"

RestorePackages()

// Read release notes & version info from RELEASE_NOTES.md
Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
let release = 
    File.ReadLines "RELEASE_NOTES.md" 
    |> ReleaseNotesHelper.parseReleaseNotes

// --------------------------------------------------------------------------------------
// Generate assembly info files with the right version & up-to-date information

Target "AssemblyInfo" (fun _ ->
    [ ("src/ExcelDnaDoc/Properties/AssemblyInfo.cs", "ExcelDnaDoc", project, summary)
      ("src/ExcelDna.Documentation/Properties/AssemblyInfo.cs", "ExcelDna.Documentation", project, summary) ]
    |> Seq.iter (fun (fileName, title, project, summary) ->
        CreateCSharpAssemblyInfo fileName
           [ Attribute.Title title
             Attribute.Product project
             Attribute.Description summary
             Attribute.Version release.AssemblyVersion
             Attribute.FileVersion release.AssemblyVersion ] )
)

// --------------------------------------------------------------------------------------
// directories

let buildFolder = "bin"
let deployFolder = "deploy"

// --------------------------------------------------------------------------------------
// Clean build results

Target "Clean" (fun _ ->
    CleanDirs [ buildFolder; deployFolder ]
)

// --------------------------------------------------------------------------------------
// Build library (builds Visual Studio solution)

Target "Build" (fun _ ->
    MSBuildRelease buildFolder "Rebuild" ["ExcelDnaDoc.sln"]
    |> Log "Build-Output: "
)

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target "NuGet" (fun _ ->
    // setup folders
    let toolsFolder = deployFolder + "/tools"
    let libFolder = deployFolder + "/lib/net40"
    let contentFolder = deployFolder + "/content"
    List.iter CreateDir [ toolsFolder; libFolder; contentFolder ]
    // tools
    !! "bin/**/*.*" 
        -- "**/*.xml"
        -- "**/*.pdb"
        ++ "nuget/install.ps1"
    |> Copy toolsFolder
    // lib
    !! "bin/ExcelDna.Documentation.dll"
    |> Copy libFolder
    // content
    !! "nuget/ExcelDnaDoc-Template.dna"
    |> Copy contentFolder
    // root
    !! "README.md"
       ++ "LICENSE.md"
    |> Copy deployFolder   

    NuGet (fun p -> 
        { p with   
            Authors = authors
            Project = project
            Summary = summary
            Description = description.Replace("\r", "").Replace("\n", "").Replace("  ", " ")
            Version = release.NugetVersion
            ReleaseNotes = release.Notes |> String.concat "\n"
            Tags = tags
            OutputPath = "deploy"
            WorkingDir = "deploy"
            ToolPath = ".nuget/nuget.exe"
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey"
            Dependencies = [("Excel-DNA", "0.30.3")] })
        "nuget/ExcelDnaDoc.nuspec"
)

// --------------------------------------------------------------------------------------
// Help

Target "Help" (fun _ ->
    printfn ""
    printfn "  Please specify the target by calling 'build <Target>'"
    printfn ""
    printfn "  Targets for building:"
    printfn "  * Build"
    printfn ""
    printfn "  Targets for releasing:"
    printfn "  * NuGet (creates package only, doesn't publish)"
    printfn "")

Target "All" DoNothing

"Clean" 
==> "AssemblyInfo" 
==> "Build" 
==> "NuGet" 
==> "All"

RunTargetOrDefault "Help"