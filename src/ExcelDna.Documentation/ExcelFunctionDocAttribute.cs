namespace ExcelDna.Documentation
{
    using ExcelDna.Integration;

    public class ExcelFunctionDocAttribute : ExcelFunctionAttribute
    {
        public string Returns = null;
        public string Summary = null;
        public string Remarks = null;

        public ExcelFunctionDocAttribute() { }

        public ExcelFunctionDocAttribute(string description)
        {
            Description = description;
        }
    }
}