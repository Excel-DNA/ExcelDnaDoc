﻿namespace ExcelDnaDoc
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ExcelDna.Documentation.Models;
    using ExcelDnaDoc.Templates;

    public static class HtmlHelp
    {
        public static string BuildFolderPath { get; set; }
        public static string HelpContentSourcePath { get; set; }
        public static string HelpContentFolderPath { get; set; }
        public static ConcurrentDictionary<string, string> TemplateCache = new ConcurrentDictionary<string, string>();

        public static void Create(AddInModel addin, string buildFolderPath, string outputFileName, string helpContentSourcePath = null, string hhcPath = null, string helpSubfolder = "HelpContent", bool excludeHidden = false, bool skipCompile = false, bool runAsync = false)
        {
            BuildFolderPath = buildFolderPath;
            HelpContentFolderPath = Path.Combine(HtmlHelp.BuildFolderPath, helpSubfolder);
            HelpContentSourcePath = helpContentSourcePath ?? HelpContentFolderPath;

            // create help content folder if it does not exist
            if (!Directory.Exists(HelpContentFolderPath)) Directory.CreateDirectory(HelpContentFolderPath);

            // HTML Help Workshop content creation
            Console.WriteLine($"Started: {DateTime.Now}");
            Console.WriteLine($"creating HTML Help content in {HelpContentFolderPath}");
            Console.WriteLine($"ExcludeHidden: {excludeHidden}, SkipCompile: {skipCompile}, Async: {runAsync}");
            Console.WriteLine();

            //Only needed for HelpWorkshop compilation
            if (!skipCompile)
            {
                new ProjectFileView { Model = addin }.Publish();
                new TableOfContentsView { Model = addin }.Publish();
            }

            new MethodListView { Model = addin }.Publish();

            //Perhaps better to actually enumerate the values?
            List<Task> tasks = new List<Task>(runAsync ? 5000 : 0);

            foreach (var group in addin.Categories)
            {
                Action categoryAction = new Action(() => new CategoryView { Model = group }.Publish());
                Run(categoryAction, tasks, runAsync);

                foreach (FunctionModel function in group.Functions)
                {
                    Action functionAction = new Action(() => new FunctionView { Model = function }.Publish());
                    Run(functionAction, tasks, runAsync);
                }
            }

            // create Excel Commands content
            if (addin.Commands.Count() != 0)
            {
                new CommandListView { Model = addin }.Publish();

                foreach (var command in addin.Commands)
                {
                    Action commandAction = new Action(() => new CommandView { Model = command }.Publish());
                    Run(commandAction, tasks, runAsync);
                }
            }

            //Will be empty if async not enabled
            Task.WaitAll(tasks.ToArray());

            // look for style sheet otherwise use embedded one

            string styleSourcePath = Path.Combine(HelpContentSourcePath, "helpstyle.css");
            string stylePath = Path.Combine(HelpContentFolderPath, "helpstyle.css");
            if (string.Compare(styleSourcePath, stylePath, true) != 0 && File.Exists(styleSourcePath))
            {
                File.Copy(styleSourcePath, stylePath, true);
            }
            if (!File.Exists(stylePath))
            {
                File.WriteAllText(stylePath, Properties.Resources.helpstyle);
            }
            else
            {
                System.Console.WriteLine("using local template : helpstyle.css");
            }

            Console.WriteLine();

            if (!skipCompile)
            {
                // compile HTML Help
                Console.WriteLine("creating chm file");
                Utility.HtmlHelpWorkshopHelper.Compile(Path.Combine(HelpContentFolderPath, outputFileName + ".hhp"), hhcPath);
                Console.WriteLine();
                Console.WriteLine();

                // move HTML Help chm file to the main build folder
                Utility.FileHelper.Move(
                    Path.Combine(HelpContentFolderPath, outputFileName + ".chm"),
                    Path.Combine(BuildFolderPath, outputFileName + ".chm"));

                Console.WriteLine();
            }

            Console.WriteLine($"Finished: {DateTime.Now}");
        }

        private static void Run(Action action, List<Task> taskList, bool runAsync)
        {
            if (runAsync)
                taskList.Add(Task.Factory.StartNew(action));
            else
                action.Invoke();
        }
    }
}