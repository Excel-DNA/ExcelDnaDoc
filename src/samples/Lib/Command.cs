using ExcelDna.Integration;

namespace Lib
{
    public class Command
    {
        [ExcelCommand(Name = "MyTestCommand", ShortCut = "^Q")]
        public static void Test()
        {
        }
    }
}
