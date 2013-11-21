// include Fake lib
#r @"tools\FAKE\tools\FakeLib.dll"

open Fake

RestorePackages()

// Directories
let buildDir = __SOURCE_DIRECTORY__ + "/sample/SampleLib/bin/Release/"

// Excel-DNA .dna path
let dnaPath = buildDir + "SampleLib-AddIn.dna"

// tools
let dnaDocRoot = @".\lib\ExcelDnaDoc.exe"
    
// Targets
Target "Clean" (fun _ -> 
    CleanDirs [buildDir]
)

Target "Build" (fun _ ->
    { BaseDirectories = [__SOURCE_DIRECTORY__]
      Includes = ["ExceDnaDocSample.sln"]
      Excludes = [] } 
    |> Scan
    |> MSBuildRelease "" "Rebuild"
    |> ignore
    )

Target "CreateHelp" (fun _ ->
    let result =
        ExecProcess (fun info -> 
            info.FileName <- dnaDocRoot
            info.Arguments <- dnaPath
        ) (System.TimeSpan.FromMinutes 1.0)     
    if result <> 0 then failwith "Operation failed or timed out"
    )

Target "BuildHelp" (fun _ ->
    let projectPath = 
        System.String.Format(@"{0}content/{1}.hhp", buildDir, System.IO.Path.GetFileNameWithoutExtension(dnaPath))
    printfn "projectPath: %s" projectPath
    let compiler = @"C:\Program Files (x86)\HTML Help Workshop\hhc.exe"
    HTMLHelpWorkShopHelper.CompileHTMLHelpProject compiler projectPath
    |> printfn "%A"
    )

// Dependencies
"Build"
==> "CreateHelp"
==> "BuildHelp"

// start build
RunTargetOrDefault "BuildHelp"