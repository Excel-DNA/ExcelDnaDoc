using ExcelDna.Documentation;
using ExcelDna.Integration;

namespace Names
{
    public class Class1
    {
        [ExcelFunction(Description = "Test")]
        public static object Test()
        {
            return "";
        }
    }
}
