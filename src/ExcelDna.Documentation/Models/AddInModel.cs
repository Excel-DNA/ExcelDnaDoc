namespace ExcelDna.Documentation.Models
{
    using System.Collections.Generic;

    public class AddInModel
    {
        public string DnaFileName { get; set; }

        public IEnumerable<FunctionGroupModel> Groups { get; set; }

        public string ProjectName { get; set; }

        public IEnumerable<FunctionModel> Functions
        {
            get
            {
                foreach (var group in this.Groups)
                {
                    foreach (var function in group.Functions)
                    {
                        yield return function;
                    }
                }          
            }

        }
    }
}