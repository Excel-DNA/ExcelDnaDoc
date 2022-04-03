using ExcelDna.Documentation;
using ExcelDna.Integration;

namespace SecondLib
{
    public class Class1
    {
        [ExcelFunctionDoc(Name = "Math.AddThem", Category = "SecondLib",
                    Description = "adds two numbers",
                    HelpTopic = "DocTest-AddIn.chm!1001",
                    Summary = "really all it does is add two number ... I promise.",
                    Example = "Math.AddThem(1, 2) returns 3",
                    Returns = "the sum of the two arguments")]
        public static int AddThem(
[ExcelArgument(Description = "the first argument")] int a,
[ExcelArgument(Description = "the second argument")] int b)
        {
            return a + b;
        }
    }
}
