namespace ExcelDnaDoc.Utility
{
    using System;
    using System.IO;

    public static class HtmlHelpWorkshopHelper
    {
        public static void Compile(string projectFile, string hhcPath)
        {
            // the HTML Help project file
            var p = new FileInfo(projectFile);
            if (!p.Exists)
            {
                throw new ArgumentException("projectFile doesn't exist", "projectFile");
            }

            // the HTML compiler
            var c = new FileInfo(hhcPath ?? @"C:\Program Files (x86)\HTML Help Workshop\hhc.exe");
            if (!c.Exists)
            {
                throw new Exception("HTML Help compiler not found, check hhc.exe path");
            }

            //"C:/Program Files (x86)/HTML Help Workshop/hhc.exe"
            ConsoleHelper.RunCommand(c.FullName, new string[] { p.FullName });

            Console.Write("finished creating HTML Help File");
        }
    }
}