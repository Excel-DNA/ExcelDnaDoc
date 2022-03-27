using ExcelDna.Documentation.Models;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelDnaDoc.Utility
{
    internal class ILInspector
    {
        public static IEnumerable<CategoryModel> GetCategories(List<Library> libraries, string defaultCategory, bool excludeHidden)
        {
            return libraries
                .SelectMany(library => ModuleDefinition.ReadModule(library.Path).Types
                    .Where(type => type.IsPublic)
                    .SelectMany(t => t.Methods)
                    .Where(m => IsValidFunction(m, library.ExplicitExports))
                    .Select(m => CreateFunctionModel(m, defaultCategory)))
                .Where(m => !excludeHidden || !m.IsHidden)
                .GroupBy(f => f.Category)
                .Select(g => new CategoryModel { Name = g.Key, Functions = g.OrderBy(f => f.Name) })
                .OrderBy(c => c.Name);
        }

        public static FunctionModel CreateFunctionModel(MethodDefinition method, string defaultCategory)
        {
            var function = new FunctionModel
            {
                Category = defaultCategory,
                Description = string.Empty,
                Name = method.Name,
                Parameters = method.Parameters.Select(p => CreateParameterModel(p)),
                ReturnType = method.ReturnType.Name,
                Returns = string.Empty,
                Summary = string.Empty,
                TopicId = string.Empty,
                Remarks = string.Empty,
                Example = string.Empty,
                IsHidden = false
            };

            var excelFunctionDoc = GetCustomAttribute(method, "ExcelDna.Documentation.ExcelFunctionDocAttribute");
            if (excelFunctionDoc != null)
            {
                function.Summary = GetField(excelFunctionDoc, "Summary") ?? function.Summary;
                function.Returns = GetField(excelFunctionDoc, "Returns") ?? function.Returns;
                function.Remarks = GetField(excelFunctionDoc, "Remarks") ?? function.Remarks;
                function.Example = GetField(excelFunctionDoc, "Example") ?? function.Example;
            }

            var excelFunction = GetCustomAttribute(method, "ExcelDna.Integration.ExcelFunctionAttribute");
            if (excelFunction != null)
            {
                function.Name = GetField(excelFunction, "Name") ?? function.Name;
                function.Description = GetField(excelFunction, "Description") ?? function.Description;
                function.Category = GetField(excelFunction, "Category") ?? function.Category;
                function.IsHidden = GetBoolField(excelFunction, "IsHidden");

                string helpTopic = GetField(excelFunction, "HelpTopic");
                if (helpTopic != null)
                    function.TopicId = helpTopic.Split('!').Last();
            }

            return function;
        }

        private static string GetField(CustomAttribute a, string name)
        {
            var field = a.Fields.FirstOrDefault(i => i.Name == name);
            if (field.Name == name)
                return field.Argument.Value as string;

            return null;
        }

        private static bool GetBoolField(CustomAttribute a, string name)
        {
            var field = a.Fields.FirstOrDefault(i => i.Name == name);
            if (field.Name == name)
                return Convert.ToBoolean(field.Argument.Value);

            return false;
        }

        private static CustomAttribute GetCustomAttribute(ICustomAttributeProvider host, string fullName)
        {
            return host.CustomAttributes.FirstOrDefault(i => IsBaseOrDerived(i.AttributeType, fullName));
        }

        private static bool IsBaseOrDerived(TypeReference type, string baseFullName)
        {
            if (type == null)
                return false;

            if (type.FullName == baseFullName)
                return true;

            return IsBaseOrDerived(type.Resolve()?.BaseType, baseFullName);
        }

        private static ParameterModel CreateParameterModel(ParameterDefinition parameter)
        {
            var model = new ParameterModel
            {
                Name = parameter.Name,
                ParameterType = parameter.ParameterType.Name,
                Description = string.Empty
            };

            var excelArgument = GetCustomAttribute(parameter, "ExcelDna.Integration.ExcelArgumentAttribute");
            if (excelArgument != null)
            {
                model.Name = GetField(excelArgument, "Name") ?? model.Name;
                model.Description = GetField(excelArgument, "Description") ?? model.Description;
            }

            model.Name = char.ToUpper(model.Name[0]) + model.Name.Substring(1);

            return model;
        }

        private static bool IsValidExcelDnaType(Type type)
        {
            var validTypes =
                new Type[] {typeof(String), typeof(DateTime), typeof(Double), typeof(Double[]), typeof(Double[,]),
                            typeof(Object), typeof(Object[]), typeof(Object[,]), typeof(Boolean), typeof(Int32),
                            typeof(Int16), typeof(UInt16), typeof(Decimal), typeof(Int64)};

            return validTypes.Any(t => type.Equals(t));
        }

        private static bool IsValidFunction(MethodDefinition method, bool explicitExports)
        {
            if (!(method.IsPublic && method.IsStatic))
            {
                return false;
            }

            if (explicitExports)
            {
                // return Attribute.IsDefined(method, typeof(ExcelFunctionAttribute));
            }

            return true;
            //var parameters = method.Parameters;
            //return (parameters.All(p => IsValidExcelDnaType(p.ParameterType))) && IsValidExcelDnaType(method.ReturnType);
        }
    }
}
