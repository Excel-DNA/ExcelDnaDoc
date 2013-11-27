namespace ExcelDnaDoc.Templates
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Reflection;
    using RazorEngine;
    using RazorEngine.Templating;

    public abstract class CustomTemplateBase<T> : TemplateBase<T>
    {
        public new T Model { get; set; }

        public abstract string PageName { get; }

        public abstract byte[] Template { get; }

        public string ViewFilePath
        {
            get
            {
                return Path.Combine(HtmlHelp.HelpContentFolderPath, this.ViewName + ".cshtml");
            }
        }

        public string ViewName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public void Publish()
        {
            // check to see if template is in cache
            if (!HtmlHelp.TemplateCache.ContainsKey(this.ViewName))
            {
                // look for razorengine template otherwise use embedded one
                if (File.Exists(this.ViewFilePath))
                {
                    Console.WriteLine("using local template : " + Path.GetFileName(this.ViewFilePath));
                    HtmlHelp.TemplateCache.Add(this.ViewName, File.ReadAllText(this.ViewFilePath));
                }
                else
                {
                    HtmlHelp.TemplateCache.Add(this.ViewName, (new UTF8Encoding()).GetString(this.Template));
                }                
            }

            // unicode result
            string content = Razor.Parse(HtmlHelp.TemplateCache[this.ViewName], this.Model);

            // strip non ANSI characters from result
            content = Regex.Replace(content, @"[^\u0000-\u007F]", string.Empty);
            content = content.TrimStart(new char[] { '\r', '\n' });

            File.WriteAllText(Path.Combine(HtmlHelp.HelpContentFolderPath, this.PageName), content);
        }
    }
}