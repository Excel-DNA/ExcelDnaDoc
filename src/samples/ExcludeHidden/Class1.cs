using ExcelDna.Documentation;
using ExcelDna.Integration;

namespace ExcludeHidden
{
    public class Class1
    {
        [ExcelFunction(Description = "visible function")]
        public static object Visible()
        {
            return "";
        }

        [ExcelFunction(Description = "hidden function", IsHidden = true)]
        public static object Hidden()
        {
            return "";
        }
    }
}
