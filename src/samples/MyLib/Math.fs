module Math

open ExcelDna.Integration
open ExcelDna.Documentation

[<ExcelFunctionDoc( Name = "Math.AddThem", Category = "Math", 
                    Description = "adds two numbers", 
                    HelpTopic = "MyAddIn.chm!1002",
                    Summary = "really all it does is add two number ... I promise.",
                    Returns = "the sum of the two arguments")>]
let addThem
    (
        [<ExcelArgument(Name = "Arg1", Description = "the first argument")>]a,
        [<ExcelArgument(Name = "Arg2", Description = "the second argument")>]b
    ) = 

    a+b