namespace ExcelDnaDoc
{
    using System;
    using System.IO;
    using System.Reflection;
    using ExcelDna.Documentation.Models;
    using ExcelDnaDoc.Templates;

    public static class HtmlHelp
    {
        public static string BuildFolderPath { get; set; }
        public static string HelpContentFolderPath { get; set; }

        public static void Create(string dnaPath, string helpSubfolder = "HelpContent")
        {
            BuildFolderPath = Path.GetDirectoryName(dnaPath);
            HelpContentFolderPath = Path.Combine(HtmlHelp.BuildFolderPath, helpSubfolder);

            // initialize data models
            var addin = Utility.ModelHelper.CreateAddInModel(dnaPath);

            // create help content folder if it does not exist
            if (!Directory.Exists(HelpContentFolderPath)) Directory.CreateDirectory(HelpContentFolderPath);

            // HTML Help Workshop content creation
            Console.WriteLine("creating HTML Help content");
            Console.WriteLine();

            new ProjectFileTemplate { Model = addin }.Publish(HelpContentFolderPath);
            new TableOfContentsTemplate { Model = addin }.Publish(HelpContentFolderPath);
            new UdfListTemplate { Model = addin }.Publish(HelpContentFolderPath);
            foreach (var group in addin.Groups) 
            {
                new FunctionGroupTemplate { Model = group }.Publish(HelpContentFolderPath);
                foreach (FunctionModel function in group.Functions) 
                {
                    new FunctionTemplate { Model = function }.Publish(HelpContentFolderPath);
                }
            }

            // look for style sheet otherwise use embedded one

            string stylePath = Path.Combine(HelpContentFolderPath, "helpstyle.css");
            if (!File.Exists(stylePath)) 
            {
                File.WriteAllText(stylePath, Properties.Resources.helpstyle); 
            }
            else 
            {
                System.Console.WriteLine("found local template: helpstyle.css"); 
            }

            // compile HTML Help

            Console.WriteLine("creating chm file");
            Utility.HtmlHelpWorkshopHelper.Compile(Path.Combine(HelpContentFolderPath, Path.GetFileNameWithoutExtension(dnaPath) + ".hhp"));
            Console.WriteLine();
            Console.WriteLine();

            // move HTML Help chm file to the main build folder
            Utility.FileHelper.Move(
                Path.Combine(HelpContentFolderPath, Path.GetFileNameWithoutExtension(dnaPath) + ".chm"),
                Path.Combine(BuildFolderPath, Path.GetFileNameWithoutExtension(dnaPath) + ".chm"));

            Console.WriteLine();
            Console.WriteLine("-- finished --");
#if DEBUG
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
#endif
        }
    }
}