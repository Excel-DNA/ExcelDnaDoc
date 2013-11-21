#r "../../distribution/RazorEngine.dll"
#r "../../packages/Excel-DNA.0.30.3/lib/ExcelDna.Integration.dll"
#r "../../distribution/ExcelDna.Documentation.dll"
#r "../../distribution/ExcelDnaDoc.dll"

let dnaPath = __SOURCE_DIRECTORY__ + "/bin/Debug/SampleLib-AddIn.dna"

ExcelDnaDoc.HtmlHelp.Create(dnaPath)