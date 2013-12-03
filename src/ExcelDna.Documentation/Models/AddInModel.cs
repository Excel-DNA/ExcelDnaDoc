namespace ExcelDna.Documentation.Models
{
    using System.Collections.Generic;

    public class AddInModel
    {
        public string DnaFileName { get; set; }

        public IEnumerable<CategoryModel> Categories { get; set; }

        public string ProjectName { get; set; }

        public IEnumerable<FunctionModel> Functions
        {
            get
            {
                var result = new List<FunctionModel>();

                foreach (var group in this.Categories)
                {
                    foreach (var function in group.Functions)
                    {
                        yield return function;
                    }
                }          
            }

        }

        public IEnumerable<CommandModel> Commands { get; set; }
    }
}