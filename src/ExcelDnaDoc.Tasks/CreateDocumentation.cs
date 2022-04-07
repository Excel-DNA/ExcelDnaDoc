using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ExcelDnaDoc.Tasks
{
    public class CreateDocumentation : ITask
    {
#if !NETFRAMEWORK
        public CreateDocumentation()
        {
            System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += AssemblyLoadResolving;
        }

        private Assembly AssemblyLoadResolving(System.Runtime.Loader.AssemblyLoadContext arg1, AssemblyName arg2)
        {
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(i => i.FullName == arg2.FullName);
        }
#endif

        public bool Execute()
        {
            try
            {
                string targetDir = Path.GetDirectoryName(TargetPath);
                string localHelpContent = Path.Combine(ProjectDirectory, "HelpContent");
                string helpContentSourcePath = Directory.Exists(localHelpContent) ? localHelpContent : null;

                var libraries = new List<Utility.Library>();
                libraries.Add(new Utility.Library() { Path = TargetPath });
                if (Include != null)
                {
                    foreach (var i in Include.Split(';'))
                        libraries.Add(new Utility.Library() { Path = Path.Combine(targetDir, i) });
                }

                string defaultCategory = DefaultCategory ?? ProjectName + " Add-In";
                string dnaFileName = AddInFileName ?? ProjectName + "-AddIn";
                string docProjectName = DocProject ?? ProjectName + " Add-In";
                var addin = Utility.ModelHelper.CreateAddInModel(libraries, defaultCategory, dnaFileName, docProjectName, ExcludeHidden);
                HtmlHelp.Create(addin, targetDir, dnaFileName, helpContentSourcePath: helpContentSourcePath, hhcPath: HHCPath, excludeHidden: ExcludeHidden, skipCompile: false);
                return true;
            }
            catch (Exception e)
            {
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs("ExcelDnaDoc exception: " + e.Message, "", "ExcelDnaDoc", MessageImportance.High));
                return false;
            }
        }

        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string ProjectDirectory { get; set; }

        [Required]
        public string TargetPath { get; set; }

        public string Include { get; set; }
        public string AddInFileName { get; set; }

        public bool ExcludeHidden { get; set; }
        public string HHCPath { get; set; }
        public string DefaultCategory { get; set; }
        public string DocProject { get; set; }
    }
}
