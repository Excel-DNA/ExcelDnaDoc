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
                string name = Path.GetFileNameWithoutExtension(TargetPath);
                bool excludeHidden = false;

                var libraries = new List<Utility.Library>();
                libraries.Add(new Utility.Library() { Path = TargetPath });
                if (Include != null)
                {
                    foreach (var i in Include.Split(';'))
                        libraries.Add(new Utility.Library() { Path = Path.Combine(targetDir, i) });
                }

                var addin = Utility.ModelHelper.CreateAddInModel(libraries, name, excludeHidden);
                HtmlHelp.Create(addin, targetDir, name, excludeHidden: excludeHidden, skipCompile: false);
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
        public string TargetPath { get; set; }

        public string Include { get; set; }
    }
}
