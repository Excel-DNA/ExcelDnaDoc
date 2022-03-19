using Microsoft.Build.Framework;
using System;

namespace ExcelDnaDoc.Tasks
{
    public class CreateDocumentation : ITask
    {
        public bool Execute()
        {
            try
            {
                HtmlHelp.Create(TargetPath, excludeHidden: false, skipCompile: false);
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
    }
}
