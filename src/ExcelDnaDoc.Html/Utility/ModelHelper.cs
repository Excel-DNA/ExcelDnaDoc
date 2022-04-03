namespace ExcelDnaDoc.Utility
{
    using System.IO;
    using System.Linq;
    using ExcelDna.Documentation.Models;
    using ExcelDna.Integration;
    using System.Collections.Generic;

    public static class ModelHelper
    {
        public static AddInModel CreateAddInModel(string dnaPath, bool excludeHidden)
        {
            var dnaLibrary = DnaLibrary.LoadFrom(dnaPath);

            List<Library> libraries = dnaLibrary.ExternalLibraries.Select(library => new Library() { Path = dnaLibrary.ResolvePath(library.Path), ExplicitExports = library.ExplicitExports }).ToList();
            string defaultCategory = dnaLibrary.Name;
            string dnaFileName = Path.GetFileNameWithoutExtension(dnaPath);
            string projectName = dnaLibrary.Name ?? dnaFileName;

            return CreateAddInModel(libraries, defaultCategory, dnaFileName, projectName, excludeHidden);
        }

        public static AddInModel CreateAddInModel(List<Library> libraries, string name, bool excludeHidden)
        {
            return CreateAddInModel(libraries, name, name, name, excludeHidden);
        }

        private static AddInModel CreateAddInModel(List<Library> libraries, string defaultCategory, string dnaFileName, string projectName, bool excludeHidden)
        {
            var model = new AddInModel();
            model.DnaFileName = dnaFileName;
            model.ProjectName = projectName;

            // process function libraries
            model.Categories = ILInspector.GetCategories(libraries, defaultCategory, excludeHidden);

            // find ExcelCommands
            model.Commands = ILInspector.GetCommands(libraries, defaultCategory);

            return model;
        }
    }
}
