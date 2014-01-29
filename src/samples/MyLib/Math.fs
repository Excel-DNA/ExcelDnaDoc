module Math

open ExcelDna.Integration
open ExcelDna.Documentation

[<ExcelFunctionDoc( Name = "Math.AddThem", Category = "Math", 
                    Description = "adds two numbers", 
                    HelpTopic = "MyAddIn.chm!1002",
                    Summary = @"All it does is add two number. See <a href=""http://en.wikipedia.org/wiki/Addition"">addition</a> for additional information.",
                    Remarks = "Inputs must be numeric otherwise an error is returned.",
                    Returns = "the sum of the two arguments")>]
let addThem
    (
        [<ExcelArgument(Name = "Arg1", Description = "the first argument")>]a,
        [<ExcelArgument(Name = "Arg2", Description = "the second argument")>]b
    ) : float = 

    a+b