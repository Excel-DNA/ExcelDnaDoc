namespace ExcelDnaDoc.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ExcelDna.Documentation;
    using ExcelDna.Integration;

    public class FunctionModel
    {
        public FunctionModel(MethodInfo method, string functionGroupName)
        {
            ExcelFunctionAttribute excelFunction;
            ExcelFunctionSummaryAttribute excelFunctionSummary;
            string name;
            string description;
            string summary;

            excelFunction = (ExcelFunctionAttribute)Attribute.GetCustomAttribute(method, typeof(ExcelFunctionAttribute));
            excelFunctionSummary = (ExcelFunctionSummaryAttribute)Attribute.GetCustomAttribute(method, typeof(ExcelFunctionSummaryAttribute));

            if (excelFunctionSummary == null)
            {
                summary = "N/A";
            }
            else
            {
                summary = excelFunctionSummary.Summary;
            }

            if (excelFunction.Name == null)
            {
                name = method.Name;
            }
            else
            {
                name = excelFunction.Name;
            }

            if (excelFunction.Description == null)
            {
                description = "N/A";
            }
            else
            {
                description = excelFunction.Description;
            }

            this.Name = name;
            this.Description = description;
            this.ReturnType = method.ReturnType.Name;
            this.Summary = summary;
            this.Parameters = method.GetParameters().Select(p => new ParameterModel(p));
            this.GroupName = functionGroupName;
        }

        public string Description { get; set; }

        public string GroupName { get; set; }

        public string Name { get; set; }

        public IEnumerable<ParameterModel> Parameters { get; set; }

        public string ReturnType { get; set; }

        public string Summary { get; set; }

        public static bool IsValidFunction(MethodInfo method)
        {
            ExcelFunctionAttribute excelFunction;
            excelFunction = (ExcelFunctionAttribute)Attribute.GetCustomAttribute(method, typeof(ExcelFunctionAttribute));

            return excelFunction != null;
        }
    }
}