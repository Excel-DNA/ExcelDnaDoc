namespace ExcelDnaDoc.Utility
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ExcelDna.Documentation;
    using ExcelDna.Documentation.Models;
    using ExcelDna.Integration;

    public static class ModelHelper
    {
        public static ParameterModel CreateParameterModel(ParameterInfo parameter)
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

            return new ParameterModel
            {
                // parameter exposed to Excel has first letter capitalized
                Name = char.ToUpper(name[0]) + name.Substring(1),

                ParameterType = parameter.ParameterType.Name,
                Description = description
            };
        }

        public static FunctionModel CreateFunctionModel(MethodInfo method, string functionGroupName)
        {
            ExcelFunctionAttribute excelFunction;
            ExcelFunctionSummaryAttribute excelFunctionSummary;
            string name;
            string description;
            string summary;
            string topicId;

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

            if (excelFunction.HelpTopic == null)
            {
                topicId = string.Empty;
            }
            else
            {
                topicId = excelFunction.HelpTopic.Split('!').Last();
            }

            return new FunctionModel
            {
                Name = name,
                Description = description,
                ReturnType = method.ReturnType.Name,
                TopidId = topicId,
                Summary = summary,
                Parameters = method.GetParameters().Select(p => CreateParameterModel(p)),
                GroupName = functionGroupName
            };
        }

        public static FunctionGroupModel CreateFunctionGroupModel(Type type)
        {
            return new FunctionGroupModel
            {
                Name = type.Name,
                Functions =
                    type.GetMethods()
                    .Where(f => IsValidFunction(f))
                    .Select(f => CreateFunctionModel(f, type.Name))
                    .OrderBy(f => f.Name)
            };
        }

        public static AddInModel CreateAddInModel(string dnaPath)
        {
            string projectName;
            var dnaPathName = Path.GetFileNameWithoutExtension(dnaPath);


            var dnaLibrary = DnaLibrary.LoadFrom(dnaPath);

            if (dnaLibrary.Name != null)
            {
                projectName = dnaLibrary.Name;
            }
            else
            {
                projectName = dnaPathName;
            }

            return new AddInModel
            {
                DnaFileName = dnaPathName,
                Groups =
                        dnaLibrary
                        .ExternalLibraries
                        .Where(e => e.ExplicitExports)
                        .Select(e => Assembly.LoadFile(dnaLibrary.ResolvePath(e.Path)))
                        .SelectMany(a => a.GetExportedTypes())
                        .Select(t => CreateFunctionGroupModel(t))
                        .Where(g => g.Functions.Count() > 0)
                        .OrderBy(g => g.Name)
            };
        }

        public static bool IsValidFunction(MethodInfo method)
        {
            ExcelFunctionAttribute excelFunction;
            excelFunction = (ExcelFunctionAttribute)Attribute.GetCustomAttribute(method, typeof(ExcelFunctionAttribute));

            return excelFunction != null;
        }
    }
}
