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
    using System.Text.RegularExpressions;

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

            var excelCommand = Attribute.GetCustomAttributes(method, typeof(ExcelCommandAttribute))
                .OfType<ExcelCommandAttribute>()
                .FirstOrDefault();

            // check if ExcelCommandAttribute used
            if (!(excelCommand is null))
            {
                if (excelCommand.Name != null) { command.Name = excelCommand.Name; }
                if (excelCommand.Description != null) { command.Description = excelCommand.Description; }
                if (excelCommand.HelpTopic != null) { command.TopicId = excelCommand.HelpTopic.Split('!').Last(); }
            }

            if (excelCommand.ShortCut != null)
            {
                Match match = Regex.Match(excelCommand.ShortCut, "^[\\^\\+\\%\\s]*([^\\^\\+\\%\\s]+)$");
                if (match.Success)
                {
                    string shortcutKeys = string.Empty;
                    if (excelCommand.ShortCut.Contains("^"))
                        shortcutKeys = "Ctrl ";
                    if (excelCommand.ShortCut.Contains("+"))
                        shortcutKeys += "Shift ";
                    if (excelCommand.ShortCut.Contains("%"))
                        shortcutKeys += "Alt ";
                    shortcutKeys = shortcutKeys.TrimStart().Replace(" ", " + ");
                    shortcutKeys += match.Groups[1].Value;
                    command.ShortCut = shortcutKeys;
                }
            }

            return command;
        }





        public static AddInModel CreateAddInModel(string dnaPath, bool excludeHidden)
        {
            string defaultCategory;
            List<Library> libraries;
            var dnaLibrary = DnaLibrary.LoadFrom(dnaPath);
            if (dnaLibrary != null)
            {
                defaultCategory = dnaLibrary.Name;
                libraries = dnaLibrary.ExternalLibraries.Select(library => new Library() { Path = dnaLibrary.ResolvePath(library.Path), ExplicitExports = library.ExplicitExports }).ToList();
            }
            else
            {
                defaultCategory = Path.GetFileNameWithoutExtension(dnaPath);
                libraries = new List<Library>();
                libraries.Add(new Library() { Path = dnaPath });
            }

            var model = new AddInModel();
            model.DnaFileName = Path.GetFileNameWithoutExtension(dnaPath);

            if (dnaLibrary?.Name != null) { model.ProjectName = dnaLibrary.Name; }
            else { model.ProjectName = model.DnaFileName; }

            // process function libraries
            model.Categories = ILInspector.GetCategories(libraries, defaultCategory, excludeHidden);

            // find ExcelCommands
            model.Commands = new List<CommandModel>();

            //model.Commands =
            //    libraries
            //    .SelectMany(library =>
            //        Assembly.Load(File.ReadAllBytes(library.Path))
            //        .GetExportedTypes()
            //        .SelectMany(t => t.GetMethods())
            //        .Where(m => ExcelDnaHelper.IsValidCommand(m))
            //        .Select(m => CreateCommandModel(m, defaultCategory)))
            //    .OrderBy(c => c.Name);

            return model;
        }
    }
}
