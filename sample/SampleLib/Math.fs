namespace SampleLib

open ExcelDna.Integration
open ExcelDna.Documentation

module Math =
    [<ExcelFunction(Description = "adds two numbers", HelpTopic="")>]
    [<ExcelFunctionSummary("really it just adds two numbers ... it doesn't do anything else")>]
    let addThem(a, b) = a + b