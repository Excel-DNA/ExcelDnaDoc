using Microsoft.Build.Framework;
using System;
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
#endif

        public bool Execute()
        {
            try
            {
                HtmlHelp.Create(TargetPath, excludeHidden: false, skipCompile: false);
                return true;
            }
            catch (Exception e)
            {
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs("ExcelDnaDoc exception: " + e.Message, "", "ExcelDnaDoc", MessageImportance.High));
                return false;
            }
        }

#if !NETFRAMEWORK
        private Assembly AssemblyLoadResolving(System.Runtime.Loader.AssemblyLoadContext arg1, AssemblyName arg2)
        {
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(i => i.FullName == arg2.FullName);
        }
#endif

        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }

        [Required]
        public string TargetPath { get; set; }
    }
}
