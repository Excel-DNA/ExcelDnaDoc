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
        public static CommandModel CreateCommandModel(MethodInfo method, string defaultCategory)
        {
            var command = new CommandModel
            {
                Name = method.Name,
                Description = string.Empty,
                ShortCut = string.Empty,
                TopicId = string.Empty,
                Category = defaultCategory
            };

            var excelCommand = (ExcelCommandAttribute)Attribute.GetCustomAttribute(method, typeof(ExcelCommandAttribute));

            // check if ExcelCommandAttribute used
            if (excelCommand != null)
            {
                if (excelCommand.Name != null) { command.Name = excelCommand.Name; }
                if (excelCommand.Description != null) { command.Description = excelCommand.Description; }
                if (excelCommand.HelpTopic != null) { command.TopicId = excelCommand.HelpTopic.Split('!').Last(); }
            }


            return command;
        }

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
                    Returns = string.Empty,
                    Summary = string.Empty,
                    TopicId = string.Empty,
                    Remarks = string.Empty
                };

            var excelFunction = (ExcelFunctionAttribute)Attribute.GetCustomAttribute(method, typeof(ExcelFunctionAttribute));
            var excelFunctionSummary = (ExcelFunctionDocAttribute)Attribute.GetCustomAttribute(method, typeof(ExcelFunctionDocAttribute));

            // check if ExcelFunctionDocAttribute used
            if (excelFunctionSummary != null)
            {
                if (excelFunctionSummary.Summary != null) { function.Summary = excelFunctionSummary.Summary; }
                if (excelFunctionSummary.Returns != null) { function.Returns = excelFunctionSummary.Returns; }
                if (excelFunctionSummary.Remarks != null) { function.Remarks = excelFunctionSummary.Remarks; } 
            }

            // check if ExcelFunctionAttribute used
            if (excelFunction != null)
            {
                if (excelFunction.Name != null) { function.Name = excelFunction.Name; }
                if (excelFunction.Description != null) { function.Description = excelFunction.Description; }
                if (excelFunction.HelpTopic != null) { function.TopicId = excelFunction.HelpTopic.Split('!').Last(); }
                if (excelFunction.Category != null) { function.Category = excelFunction.Category; }
            }

            return function;
        }

        public static AddInModel CreateAddInModel(string dnaPath, bool excludeHidden)
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
                    Assembly.LoadFrom(dnaLibrary.ResolvePath(library.Path))
                    .GetExportedTypes()
                    .SelectMany(t => t.GetMethods())
                    .Where(m => ExcelDnaHelper.IsValidFunction(m, library.ExplicitExports))
                    .Select(m => CreateFunctionModel(m, defaultCategory)))
                    .Where(m => excludeHidden && !m.IsHidden) //Used to exclude hidden functions
                .GroupBy(f => f.Category)
                .Select(g => new CategoryModel { Name = g.Key, Functions = g.OrderBy(f => f.Name) })
                .OrderBy(c => c.Name);

            // find ExcelCommands

            model.Commands =
                dnaLibrary
                .ExternalLibraries
                .SelectMany(library =>
                    Assembly.LoadFrom(dnaLibrary.ResolvePath(library.Path))
                    .GetExportedTypes()
                    .SelectMany(t => t.GetMethods())
                    .Where(m => ExcelDnaHelper.IsValidCommand(m))
                    .Select(m => CreateCommandModel(m, defaultCategory)))
                .OrderBy(c => c.Name);

            return model;
        }
    }
}
