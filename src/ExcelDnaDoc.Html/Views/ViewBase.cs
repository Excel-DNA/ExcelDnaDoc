namespace ExcelDnaDoc.Templates
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

#if NETFRAMEWORK
    using RazorEngine;
    using RazorEngine.Templating;
#else
    using RazorEngineCore;
#endif

#if NETFRAMEWORK
    public abstract class ViewBase<T> : TemplateBase<T>
#else
    public abstract class ViewBase<T>
#endif
    {
        public new T Model { get; set; }

        public abstract string PageName { get; }

        public abstract byte[] Template { get; }

        public string ViewFilePath
        {
            get
            {
                return Path.Combine(HtmlHelp.HelpContentFolderPath, this.TemplateName + ".cshtml");
            }
        }

        public string TemplateName
        {
            get
            {
                return this.GetType().Name.Replace("View", "Template");
            }
        }

        public void Publish()
        {
#if NETFRAMEWORK
            // check to see if template is in cache
            string cacheItem = HtmlHelp.TemplateCache.GetOrAdd(this.TemplateName, k =>
            {
                // look for razorengine template otherwise use embedded one
                if (File.Exists(this.ViewFilePath))
                {
                    Console.WriteLine("using local template : " + Path.GetFileName(this.ViewFilePath));
                    return File.ReadAllText(this.ViewFilePath);
                }
                else
                {
                    return (new UTF8Encoding()).GetString(this.Template);
                }
            });

            // unicode result
            string content = Engine.Razor.RunCompile(cacheItem, this.TemplateName, null, this.Model); // Razor.Parse(cacheItem, this.Model);
#else
            IRazorEngine razorEngine = new RazorEngine();
            string templateText = (new UTF8Encoding()).GetString(this.Template);

            templateText = templateText.Replace("@model ExcelDna.Documentation.Models.AddInModel", "");
            templateText = templateText.Replace("@model ExcelDna.Documentation.Models.CategoryModel", "");
            templateText = templateText.Replace("@model ExcelDna.Documentation.Models.FunctionModel", "");
            templateText = templateText.Replace("@Raw", "");

            IRazorEngineCompiledTemplate<RazorEngineTemplateBase<T>> template = razorEngine.Compile<RazorEngineTemplateBase<T>>(templateText);

            string content = template.Run(instance =>
            {
                instance.Model = Model;
            });
#endif
            // strip non ANSI characters from result
            content = Regex.Replace(content, @"[^\u0000-\u007F]", string.Empty);
            content = content.TrimStart(new char[] { '\r', '\n' });

            File.WriteAllText(Path.Combine(HtmlHelp.HelpContentFolderPath, this.PageName), content);
        }
    }
}