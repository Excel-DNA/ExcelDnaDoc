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
    }
}
