using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExcelDnaDoc.Utility
{
    public static class HtmlHelpWorkshopHelper
    {
        public static void Compile(string projectFile)
        {
            // the HTML Help project file
            var p = new FileInfo(projectFile);
            if (!p.Exists)
            {
                throw new ArgumentException("projectFile doesn't exist", "projectFile");
            }
            
            // the HTML compiler
            var c = new FileInfo(ConfigurationManager.AppSettings["HtmlHelpWorkshopCompilerPath"]);
            if (!c.Exists)
            {
                throw new Exception("HTML Help compiler not found, check .config setting");
            }

            //"C:/Program Files (x86)/HTML Help Workshop/hhc.exe"
            ConsoleHelper.RunCommand(c.FullName, new string[] { p.FullName });

            Console.Write("finished creating HTML Help File");
        }
    }
}