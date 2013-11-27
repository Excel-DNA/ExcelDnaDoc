namespace ExcelDnaDoc.Utility
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ExcelDna.Documentation;
    using ExcelDna.Documentation.Models;
    using ExcelDna.Integration;
    using System.Collections.Generic;

    public static class ModelHelper
    {
        public static ParameterModel CreateParameterModel(ParameterInfo parameter)
        {
            var model = new ParameterModel
            {
                Name = parameter.Name,
                ParameterType = parameter.ParameterType.Name,
                Description = string.Empty
            };

            var excelArgument = (ExcelArgumentAttribute)Attribute.GetCustomAttribute(parameter, typeof(ExcelArgumentAttribute));

            if (excelArgument != null)
            {
                if (excelArgument.Name != null) { model.Name = excelArgument.Name; }
                if (excelArgument.Description != null) { model.Description = excelArgument.Description; }
            }

            model.Name = char.ToUpper(model.Name[0]) + model.Name.Substring(1);

            return model;
        }

        public static FunctionModel CreateFunctionModel(MethodInfo method, string defaultCategory)
        {
            var function =
                new FunctionModel
                {
                    Category = defaultCategory,
                    Description = string.Empty,
                    Name = method.Name,
                    Parameters = method.GetParameters().Select(p => CreateParameterModel(p)),
                    ReturnType = method.ReturnType.Name,
                    Summary = string.Empty,
                    TopidId = string.Empty
                };

            var excelFunction = (ExcelFunctionAttribute)Attribute.GetCustomAttribute(method, typeof(ExcelFunctionAttribute));
            var excelFunctionSummary = (ExcelFunctionSummaryAttribute)Attribute.GetCustomAttribute(method, typeof(ExcelFunctionSummaryAttribute));

            // check if ExcelFunctionSummaryAttribute used
            if (excelFunctionSummary != null)
            {
                if (excelFunctionSummary.Summary != null) { function.Summary = excelFunctionSummary.Summary; }
            }

            // check if ExcelFunctionAttribute used
            if (excelFunction != null)
            {
                if (excelFunction.Name != null) { function.Name = excelFunction.Name; }
                if (excelFunction.Description != null) { function.Description = excelFunction.Description; }
                if (excelFunction.HelpTopic != null) { function.TopidId = excelFunction.HelpTopic.Split('!').Last(); }
                if (excelFunction.Category != null) { function.Category = excelFunction.Category; }
            }

            return function;
        }

        public static AddInModel CreateAddInModel(string dnaPath)
        {
            var model = new AddInModel();
            var dnaLibrary = DnaLibrary.LoadFrom(dnaPath);
            var defaultCategory = dnaLibrary.Name;

            //var functions = new List<FunctionModel>();

            model.DnaFileName = Path.GetFileNameWithoutExtension(dnaPath);

            if (dnaLibrary.Name != null) {model.ProjectName = dnaLibrary.Name;}
            else { model.ProjectName = model.DnaFileName; }

            // process function libraries

            model.Categories =
                dnaLibrary
                .ExternalLibraries
                .SelectMany(library =>
                    Assembly.LoadFile(dnaLibrary.ResolvePath(library.Path))
                    .GetExportedTypes()
                    .SelectMany(t => t.GetMethods())
                    .Where(m => ExcelDnaHelper.IsValidFunction(m, library.ExplicitExports))
                    .Select(m => CreateFunctionModel(m, defaultCategory)))
                .GroupBy(f => f.Category)
                .Select(g => new CategoryModel { Name = g.Key, Functions = g.OrderBy(f => f.Name) })
                .OrderBy(c => c.Name);

            return model;
        }
    }
}
