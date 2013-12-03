namespace ExcelDnaDoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ExcelDna.Documentation.Models;
    using ExcelDnaDoc.Templates;

    public static class HtmlHelp
    {
        public static string BuildFolderPath { get; set; }
        public static string HelpContentFolderPath { get; set; }
        public static Dictionary<string, string> TemplateCache = new Dictionary<string, string>();

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

            new ProjectFileView { Model = addin }.Publish();
            new TableOfContentsView { Model = addin }.Publish();
            new MethodListView { Model = addin }.Publish();

            foreach (var group in addin.Categories) 
            {
                new CategoryView { Model = group }.Publish();

                foreach (FunctionModel function in group.Functions) 
                {
                    new FunctionView { Model = function }.Publish();
                }
            }

            // create Excel Commands content

            if (addin.Commands.Count() != 0)
            {
                new CommandListView { Model = addin }.Publish();

                foreach (var command in addin.Commands)
                {
                    new CommandView { Model = command }.Publish();
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
                System.Console.WriteLine("using local template : helpstyle.css"); 
            }

            Console.WriteLine();

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