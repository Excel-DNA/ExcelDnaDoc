using ExcelDna.Documentation;
using ExcelDna.Integration;

namespace LibNET4
{
    public class Class1
    {
        [ExcelFunction(Name = "Text.ConcatThem",
                Description = "concatenates two strings",
                HelpTopic = "DocTest-AddIn.chm!1002")]
        public static object ConcatThem(
    [ExcelArgument(Description = "the first string")] object a,
    [ExcelArgument(Description = "the second string")] object b)
        {
            return string.Concat(a.ToString(), b.ToString());
        }

        [ExcelFunctionDoc(Name = "Math.AddThem", Category = "Math",
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
