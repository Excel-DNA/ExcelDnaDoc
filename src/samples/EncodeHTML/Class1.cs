using ExcelDna.Documentation;
using ExcelDna.Integration;

namespace EncodeHTML
{
    public class Class1
    {
        [ExcelFunction(Description = "<b>Test</b>")]
        public static object Escaped()
        {
            return "";
        }

        [ExcelFunctionDoc(Example = "<b>Test</b>")]
        public static int Raw()
        {
            return 0;
        }
    }
}
