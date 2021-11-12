namespace MyAddIn
{
    using ExcelDna.Integration;

    public class Text
    {
        [ExcelFunction( Name = "Text.ConcatThem15", 
                        Description = "concatenates two strings", 
                        HelpTopic = "MyAddIn.chm!1001")]
        public static object ConcatThem(
            [ExcelArgument(Description="the first string")] object a, 
            [ExcelArgument(Description="the second string")] object b)
        {
            return string.Concat(a.ToString(), b.ToString());
        }
    }
}
