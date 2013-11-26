namespace ExcelDnaDoc
{
    using System.IO;
    using System.Reflection;
    using ExcelDna.Documentation.Models;
    using ExcelDnaDoc.Templates;

    public static class HtmlHelp
    {
        public static string BuildFolderPath {get;set;}

        public static void Create(string dnaPath, string helpSubfolder = "content")
        {
            HtmlHelp.BuildFolderPath = Path.GetDirectoryName(dnaPath);

            var addin = Utility.ModelHelper.CreateAddInModel(dnaPath);

            var helpFolderPath = Path.Combine(HtmlHelp.BuildFolderPath, helpSubfolder);

            // delete and recreate the destination directory
            if (Directory.Exists(helpFolderPath)) Directory.Delete(helpFolderPath, true);
            Directory.CreateDirectory(helpFolderPath);

            // HTML Help Workshop File Creation

            // create Help Project File
            new ProjectFileTemplate { Model = addin }.Publish(helpFolderPath);

            // create Table of Contents Page
            new TableOfContentsTemplate { Model = addin }.Publish(helpFolderPath);

            // create UDF List Page
            new UdfListTemplate { Model = addin }.Publish(helpFolderPath);

            // create Function and Function Group Pages
            foreach (var group in addin.Groups)
            {
                new FunctionGroupTemplate { Model = group }.Publish(helpFolderPath);

                foreach (FunctionModel function in group.Functions)
                {
                    new FunctionTemplate { Model = function }.Publish(helpFolderPath);
                }
            }

            // look for style sheet otherwise use embedded one
            string styleContent;
            string stylePath = 
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                             "Views/helpstyle.css");

            if (File.Exists(stylePath))
            {
                styleContent = File.ReadAllText(stylePath);                
            }
            else
            {
                styleContent = Properties.Resources.helpstyle;
            }

            File.WriteAllText(Path.Combine(helpFolderPath, "helpstyle.css"), styleContent);
        }
    }
}