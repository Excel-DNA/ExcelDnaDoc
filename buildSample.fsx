// include Fake lib
#r @"tools\FAKE\tools\FakeLib.dll"

open System
open System.IO
open Fake

RestorePackages()

// Directories
let buildDir = Path.Combine( __SOURCE_DIRECTORY__, "sample/SampleLib/bin/Release")

// DNA name
let dnaName = "SampleLib-AddIn"

// tools
let dnaDocRoot = "./lib/ExcelDnaDoc.exe"
let helpCompilerRoot = "C:/Program Files (x86)/HTML Help Workshop/hhc.exe"
    
// Targets
Target "Clean" (fun _ -> 
    CleanDirs [buildDir]
    )

Target "Build" (fun _ ->
    { BaseDirectories = [__SOURCE_DIRECTORY__]
      Includes = ["ExcelDnaDocSample.sln"]
      Excludes = [] } 
    |> Scan
    |> MSBuildRelease "" "Rebuild"
    |> Log "Build-Output: "
    )

Target "CreateHelp" (fun _ ->
    let result =
        ExecProcess (fun info -> 
            info.FileName <- dnaDocRoot
            info.Arguments <- Path.Combine(buildDir, dnaName + ".dna")
        ) (System.TimeSpan.FromMinutes 1.0)     
    if result <> 0 then failwith "Operation failed or timed out"
    )

Target "BuildHelp" (fun _ ->
    let projectPath = 
        Path.Combine(buildDir, String.Format("content/{0}.hhp", dnaName))
    let compiler = helpCompilerRoot
    HTMLHelpWorkShopHelper.CompileHTMLHelpProject compiler projectPath
    |> Log "BuildHelp-Output: "
    [ Path.Combine(buildDir, String.Format("content/{0}.chm", dnaName)) ]
    |> Copy buildDir
    )

// Dependencies
"Build"
==> "CreateHelp"
==> "BuildHelp"


// start build
RunTargetOrDefault "BuildHelp"