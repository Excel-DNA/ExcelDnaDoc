using Microsoft.Build.Framework;
using System;
using System.Diagnostics;

namespace ExcelDnaDoc.Tasks
{
    public class CreateDocumentation : ITask
    {
        public bool Execute()
        {
            try
            {
                Process.Start(ExcelDnaDocPath, TargetPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }

        [Required]
        public string TargetPath { get; set; }

        [Required]
        public string ExcelDnaDocPath { get; set; }
    }
}
