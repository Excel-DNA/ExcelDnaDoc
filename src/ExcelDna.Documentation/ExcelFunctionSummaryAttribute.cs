namespace ExcelDna.Documentation
{
    public class ExcelFunctionSummaryAttribute : System.Attribute
    {
        public ExcelFunctionSummaryAttribute(string summary)
        {
            this.Summary = summary;
        }
        
        public string Summary { get; set; }
    }
}