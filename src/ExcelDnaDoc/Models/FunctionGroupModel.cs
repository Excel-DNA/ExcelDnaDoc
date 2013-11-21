namespace ExcelDnaDoc.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FunctionGroupModel
    {
        public FunctionGroupModel(Type type)
        {
            this.Name = type.Name;
            this.Functions =
                type.GetMethods()
                .Where(f => FunctionModel.IsValidFunction(f))
                .Select(f => new FunctionModel(f, type.Name))
                .OrderBy(f => f.Name);
        }

        public IEnumerable<FunctionModel> Functions { get; set; }

        public string Name { get; set; }
    }
}