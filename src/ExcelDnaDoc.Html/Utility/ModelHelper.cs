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
            model.Commands = ILInspector.GetCommands(libraries, defaultCategory);

            return model;
        }
    }
}
