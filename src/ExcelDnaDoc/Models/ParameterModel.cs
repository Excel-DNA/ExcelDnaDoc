namespace ExcelDnaDoc.Models
{
    using System;
    using System.Reflection;
    using ExcelDna.Integration;

    public class ParameterModel
    {
        public ParameterModel(ParameterInfo parameter)
        {
            ExcelArgumentAttribute excelArgument;
            string description;
            string name;

            excelArgument =
                (ExcelArgumentAttribute)
                Attribute.GetCustomAttribute(parameter, typeof(ExcelArgumentAttribute));

            if (excelArgument == null)
            {
                name = parameter.Name;
            }
            else if (excelArgument.Name == null)
            {
                name = parameter.Name;
            }
            else
            {
                name = excelArgument.Name;
            }

            if (excelArgument == null)
            {
                description = "N/A";
            }
            else if (excelArgument.Description == null)
            {
                description = "N/A";
            }
            else
            {
                description = excelArgument.Description;
            }

            // parameter exposed to Excel has first letter capitalized
            this.Name = char.ToUpper(name[0]) + name.Substring(1);

            this.ParameterType = parameter.ParameterType.Name;
            this.Description = description;
        }

        public string Description { get; set; }

        public string Name { get; set; }

        public string ParameterType { get; set; }
    }
}