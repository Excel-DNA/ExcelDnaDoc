// --------------------------------------------------------------------------------------
// Builds the documentation from `.fsx` and `.md` files in the 'docs/content' directory
// (the generated documentation is stored in the 'docs/output' directory)
// --------------------------------------------------------------------------------------

// Binaries that have XML documentation (in a corresponding generated XML file)
let referenceBinaries = [] // [ "ExcelDna.Documentation.dll" ]
// Web site location for the generated documentation
let website = "/ExcelDnaDoc"

// Specify more information about your project
let info =
  [ "project-name", "ExcelDnaDoc"
    "project-author", "David Carlson"
    "project-summary", "command-line utility to create a compiled HTML Help Workshop file (.chm) for ExcelDna"
    "project-github", "http://github.com/mndrake/ExcelDnaDoc"
    "project-nuget", "http://nuget.org/packages/ExcelDnaDoc" ]

// --------------------------------------------------------------------------------------
// For typical project, no changes are needed below
// --------------------------------------------------------------------------------------



#r "nuget: RazorEngine, 3.3.0"
#r "nuget: FSharp.Formatting, 2.13.6"
#r "nuget: Fake.IO.FileSystem, 5.20.4"
#r "nuget: Fake.Core.Trace, 5.20.4"


open Fake.IO
open Fake.Core
open System.IO
open FSharp.Literate
open FSharp.MetadataFormat

open Fake.IO.FileSystemOperators

// When called from 'build.fsx', use the public project URL as <root>
// otherwise, use the current 'output' directory.
#if RELEASE
printfn "RELEASE is defined"
let root = website
#else
printfn "RELEASE is not defined"
let root = "file://" + (__SOURCE_DIRECTORY__ @@ "../output")
#endif

// Paths with template/source/output locations
let bin        = __SOURCE_DIRECTORY__ @@ "../../bin"
let content    = __SOURCE_DIRECTORY__ @@ "../content"
let output     = __SOURCE_DIRECTORY__ @@ "../output"
let files      = __SOURCE_DIRECTORY__ @@ "../files"
let templates  = __SOURCE_DIRECTORY__ @@ "templates"
let formatting = __SOURCE_DIRECTORY__ @@ "../../packages/FSharp.Formatting.2.13.6/"
let docTemplate = formatting @@ "templates/docpage.cshtml"

// Where to look for *.csproj templates (in this order)
let layoutRoots =
  [ templates; formatting @@ "templates"
    formatting @@ "templates/reference" ]

// Copy static files and CSS + JS from F# Formatting
let copyFiles () =
  Shell.copyRecursive files output true |> Trace.logItems "Copying file: " |> ignore
  Directory.ensure (output @@ "content" )
  Shell.copyRecursive (formatting @@ "styles") (output @@ "content") true 
    |> Trace.logItems "Copying styles and scripts: "

// Build API reference from XML comments
let buildReference () =
  Shell.cleanDir (output @@ "reference")
  for lib in referenceBinaries do
    MetadataFormat.Generate
      ( bin @@ lib, output @@ "reference", layoutRoots, 
        parameters = ("root", root)::info )

// Build documentation from `fsx` and `md` files in `docs/content`
let buildDocumentation () =
  let subdirs = Directory.EnumerateDirectories(content, "*", SearchOption.AllDirectories)
  for dir in Seq.append [content] subdirs do
    let sub = if dir.Length > content.Length then dir.Substring(content.Length + 1) else "."
    Literate.ProcessDirectory
      ( dir, docTemplate, output @@ sub, replacements = ("root", root)::info,
        layoutRoots = layoutRoots )

// Generate
copyFiles()
buildDocumentation()
buildReference()
