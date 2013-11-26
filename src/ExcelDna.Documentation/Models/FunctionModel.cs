namespace ExcelDna.Documentation.Models
{
    using System.Collections.Generic;

    public class FunctionModel
    {
        public string Description { get; set; }

        public string GroupName { get; set; }

        public string Name { get; set; }

        public IEnumerable<ParameterModel> Parameters { get; set; }

        public string ReturnType { get; set; }

        public string TopidId { get; set; }

        public string Summary { get; set; }
    }
}