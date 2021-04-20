// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------

#r "paket:
nuget Fake.DotNet.MsBuild
nuget Fake.DotNet.NuGet
nuget Fake.DotNet.Fsi
nuget Fake.DotNet.AssemblyInfoFile
nuget Fake.Core.Target
nuget Fake.Core.ReleaseNotes
nuget Fake.Core.Environment
nuget Fake.IO.FileSystem
nuget Fake.Tools.Git
//"
#load "./.fake/build.fsx/intellisense.fsx"

open System
open System.IO
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.DotNet.NuGet
open Fake.Tools.Git



// --------------------------------------------------------------------------------------
// Information about the project to be used at NuGet and in AssemblyInfo files
// --------------------------------------------------------------------------------------

let project = "ExcelDnaDoc"
let authors = ["David Carlson, Excel-DNA Developers"]
let summary = "command-line utility to create a compiled HTML Help Workshop file (.chm) for ExcelDna"
let description = """
  Command-line utility to create a compiled HTML Help Workshop file (.chm) for ExcelDna. 
  To build compiled help file (.chm) the HTML Help Workshop must be installed. 
  For examples see https://github.com/Excel-DNA/ExcelDnaDoc."""
let tags = "Excel-DNA Excel"

let gitHome = "https://github.com/Excel-DNA"
let gitName = "ExcelDnaDoc"

Target.create "NuGetRestore" (fun _ ->
    Restore.RestorePackages ()
)

// Read release notes & version info from RELEASE_NOTES.md
Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
let release = 
    File.ReadLines "RELEASE_NOTES.md" 
    |> ReleaseNotes.parse

// --------------------------------------------------------------------------------------
// Generate assembly info files with the right version & up-to-date information

Target.create "AssemblyInfo" (fun _ ->
    [ ("src/ExcelDnaDoc/Properties/AssemblyInfo.cs", "ExcelDnaDoc", project, summary)
      ("src/ExcelDna.Documentation/Properties/AssemblyInfo.cs", "ExcelDna.Documentation", project, summary) ]
    |> Seq.iter (fun (fileName, title, project, summary) ->
        AssemblyInfoFile.createCSharp fileName
            [ AssemblyInfo.Title title
              AssemblyInfo.Product project
              AssemblyInfo.Description summary
              AssemblyInfo.Version release.AssemblyVersion
              AssemblyInfo.FileVersion release.AssemblyVersion ] )
)

// --------------------------------------------------------------------------------------
// Clean build results

Target.create "Clean" (fun _ -> Shell.cleanDirs ["bin";"temp"])

Target.create "CleanDocs" (fun _ -> Shell.cleanDirs ["docs/output"])

// --------------------------------------------------------------------------------------
// Build library (builds Visual Studio solution)

Target.create "Build" (fun _ ->
    [ "src/ExcelDna.Documentation/ExcelDna.Documentation.csproj"
      "src/ExcelDnaDoc/ExcelDnaDoc.csproj" ]
    |> MSBuild.runRelease id "bin" "Rebuild"
    |> ignore
//    |> Log "Build-Output: "
)

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target.create "NuGet" (fun _ ->
    NuGet.NuGet (fun p -> 
        { p with   
            Authors = authors
            Project = project
            Summary = summary
            Description = description.Replace("\r", "").Replace("\n", "").Replace("  ", " ")
            Version = release.NugetVersion
            ReleaseNotes = release.Notes |> String.concat "\n"
            Tags = tags
            OutputPath = "bin"
            ToolPath = ".nuget/nuget.exe"
            AccessKey = Environment.environVarOrDefault "nugetkey" ""
            Publish = Environment.hasEnvironVar "nugetkey"
            Dependencies = [("ExcelDna.Integration", "1.1.0")] })
        "nuget/ExcelDnaDoc.nuspec"
)

// --------------------------------------------------------------------------------------
// Generate the documentation

Target.create "GenerateDocs" (fun _ ->
    Fsi.exec (fun p -> 
        { p with 
            WorkingDirectory = "docs/tools"
            // ToolPath = FsiTool.External fsiExe
        })
        "generate.fsx" ["--define:RELEASE"] |> ignore
)


// --------------------------------------------------------------------------------------
// Release Scripts

Target.create "ReleaseDocs" (fun _ ->
    Repository.clone "" (gitHome + "/" + gitName + ".git") "temp/gh-pages"
    Branches.checkoutBranch "temp/gh-pages" "gh-pages"
    Shell.copyRecursive "docs/output" "temp/gh-pages" true |> printfn "%A"
    CommandHelper.runSimpleGitCommand "temp/gh-pages" "add ." |> printfn "%s"
    let cmd = sprintf """commit -a -m "Update generated documentation for version %s""" release.NugetVersion
    CommandHelper.runSimpleGitCommand "temp/gh-pages" cmd |> printfn "%s"
    Branches.push "temp/gh-pages"
)

Target.create "ReleaseBinaries" (fun _ ->
    Repository.clone "" (gitHome + "/" + gitName + ".git") "temp/release"
    Branches.checkoutBranch "temp/release" "release"
    Shell.copyRecursive "bin" "temp/release" true |> printfn "%A"
    CommandHelper.runSimpleGitCommand "temp/release" "add ." |> printfn "%s"
    let cmd = sprintf """commit -a -m "Update binaries for version %s""" release.NugetVersion
    CommandHelper.runSimpleGitCommand "temp/release" cmd |> printfn "%s"
    Branches.push "temp/release"
)

// --------------------------------------------------------------------------------------
// Help

Target.create "Help" (fun _ ->
    printfn ""
    printfn "  Please specify the target by calling 'build <Target>'"
    printfn ""
    printfn "  Targets for building:"
    printfn "  * Build"
    printfn "  * All (calls previous 1)"
    printfn ""
    printfn "  Targets for releasing:"
    printfn "  * GenerateDocs"
    printfn "  * ReleaseDocs (calls previous)"
    printfn "  * ReleaseBinaries"
    printfn "  * NuGet (creates package only, doesn't publish)"
    printfn "  * Release (calls previous 4)"
    printfn "  * DryRunRelease"
    printfn "")


Target.create "All" 
    ignore
Target.create "DryRunRelease"
    ignore
Target.create "Release"
    ignore


open Fake.Core.TargetOperators

"Clean" ==> "AssemblyInfo"  ==> "Build" 
"Clean" ==> "NuGetRestore" ==> "Build"
"Build" ==> "CleanDocs" ==> "GenerateDocs" ==> "ReleaseDocs"
"Build" ==> "NuGet" ==> "ReleaseBinaries"


"Build" ==> "All"

"GenerateDocs" ==> "DryRunRelease"
"NuGet" ==> "DryRunRelease"

"ReleaseBinaries" ==> "Release"
"ReleaseDocs" ==> "Release"



Target.runOrDefaultWithArguments "Help"
