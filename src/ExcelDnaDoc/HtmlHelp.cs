namespace ExcelDnaDoc
{
    using System.IO;
    using System.Reflection;
    using ExcelDnaDoc.Models;
    using ExcelDnaDoc.ViewModels;

    public static class HtmlHelp
    {
        public static void Create(string dnaPath, string helpFolder = "content")
        {
            var addin = new AddInModel(dnaPath);
            var helpDirectoryPath = Path.Combine((new FileInfo(dnaPath)).Directory.FullName, helpFolder);

            // delete and recreate the destination directory
            if (Directory.Exists(helpDirectoryPath)) 
            {
                Directory.Delete(helpDirectoryPath, true);
            }

            Directory.CreateDirectory(helpDirectoryPath);

            // HTML Help Workshop File Creation

            // create Help Project File
            new ProjectFileViewModel { Model = addin }.Publish(helpDirectoryPath);

            // create Table of Contents Page
            new TableOfContentsViewModel { Model = addin }.Publish(helpDirectoryPath);

            // create UDF List Page
            new UdfListViewModel { Model = addin }.Publish(helpDirectoryPath);

            // create Function and Function Group Pages
            foreach (var group in addin.Groups)
            {
                new FunctionGroupViewModel { Model = group }.Publish(helpDirectoryPath);

                foreach (FunctionModel function in group.Functions)
                {
                    new FunctionViewModel { Model = function }.Publish(helpDirectoryPath);
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

            File.WriteAllText(Path.Combine(helpDirectoryPath, "helpstyle.css"), styleContent);
        }
    }
}