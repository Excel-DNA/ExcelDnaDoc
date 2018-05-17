namespace ExcelDna.Documentation.Models
{
    using System.Collections.Generic;

    public class FunctionModel
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public IEnumerable<ParameterModel> Parameters { get; set; }

        public string ReturnType { get; set; }

        public string Returns { get; set; }

        public string TopicId { get; set; }

        public string Summary { get; set; }

        public string Remarks { get; set; }

        public string Example { get; set; }

        public bool IsHidden { get; set; }
    }
}