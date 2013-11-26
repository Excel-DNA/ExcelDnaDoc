namespace ExcelDna.Documentation.Models
{
    using System.Collections.Generic;

    public class FunctionGroupModel
    {
        public IEnumerable<FunctionModel> Functions { get; set; }

        public string Name { get; set; }
    }
}