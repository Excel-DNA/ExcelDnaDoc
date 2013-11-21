namespace ExcelDnaDoc.Models
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ExcelDna.Integration;

    public class AddInModel
    {
        public AddInModel(string dnaPath)
        {
            var dnaPathName = Path.GetFileNameWithoutExtension(dnaPath);
            var dnaLibrary = DnaLibrary.LoadFrom(dnaPath);

            if (dnaLibrary.Name != null)
            {
                this.ProjectName = dnaLibrary.Name;
            }
            else
            {
                this.ProjectName = dnaPathName;
            }

            this.DnaFileName = dnaPathName;

            this.Groups =
                    dnaLibrary
                    .ExternalLibraries
                    .Where(e => e.ExplicitExports)
                    .Select(e => Assembly.LoadFile(dnaLibrary.ResolvePath(e.Path)))
                    .SelectMany(a => a.GetExportedTypes())
                    .Select(t => new FunctionGroupModel(t))
                    .Where(g => g.Functions.Count() > 0)
                    .OrderBy(g => g.Name);
        }

        public string DnaFileName { get; set; }

        public IEnumerable<FunctionGroupModel> Groups { get; set; }

        public string ProjectName { get; set; }

        public IEnumerable<FunctionModel> Functions
        {
            get
            {
                foreach (var group in this.Groups)
                {
                    foreach (var function in group.Functions)
                    {
                        yield return function;
                    }
                }          
            }

        }
    }
}