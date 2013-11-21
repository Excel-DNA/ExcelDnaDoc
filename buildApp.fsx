// include Fake lib
#r @"tools\FAKE\tools\FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile

RestorePackages()

// Directories
let buildDir  = @".\build\"
let deployDir = @".\deploy\"
let packagesDir = @".\packages"

// tools
let fxCopRoot = @".\Tools\FxCop\FxCopCmd.exe"
    
// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ -> 
    CleanDirs [buildDir; deployDir]
)

Target "SetVersions" (fun _ ->
    CreateCSharpAssemblyInfo "./src/ExcelDnaDoc/Properties/AssemblyInfo.cs"
        [Attribute.Title "ExcelDna Documentation"
         Attribute.Description "HTML Help Documentation Generator for ExcelDna"
         Attribute.Guid "297C9DE2-B4FA-4268-991D-F426C23BFED1"
         Attribute.Product "ExcelDnaDoc"
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./src/ExcelDna.Documentation/Properties/AssemblyInfo.cs"
        [Attribute.Title "ExcelDna Documentation"
         Attribute.Description "HTML Help Documentation Summary Attribute for ExcelDna"
         Attribute.Guid "3DC4493F-030F-404D-90B3-9A1B7E1757FC"
         Attribute.Product "ExcelDna.Documentation"
         Attribute.Version version
         Attribute.FileVersion version]
)

Target "Build" (fun _ ->
    { BaseDirectories = [__SOURCE_DIRECTORY__]
      Includes = ["ExcelDnaDoc.sln"]
      Excludes = [] }   
    |> Scan
    |> MSBuildRelease buildDir "Rebuild"
    |> Log "Build-Output: "
    )

Target "Zip" (fun _ ->
    !+ (buildDir + "\**\*.*") 
        -- "*.zip" 
        |> Scan
        |> Zip buildDir (deployDir + "ExcelDnaDoc." + version + ".zip")
)

// Dependencies
"Clean"
  ==> "SetVersions" 
  ==> "Build"  
//  ==> "Zip"
 
// start build
RunTargetOrDefault "Build"