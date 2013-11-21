namespace ExcelDnaDoc.ViewModels
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using RazorEngine;
    using RazorEngine.Templating;

    public abstract class ViewModelBase<T> : TemplateBase<T>
    {
        public ViewModelBase()
        {
        }

        public new T Model { get; set; }

        public abstract string PageName { get; }

        public abstract byte[] Template { get; }

        public void Publish(string folder)
        {
            // destination file path
            string path = Path.Combine(folder, this.PageName);

            // razorengine template
            string template = (new UTF8Encoding()).GetString(this.Template);

            // unicode result
            string content = Razor.Parse(template, this.Model);

            // strip non ANSI characters from result
            content = Regex.Replace(content, @"[^\u0000-\u007F]", string.Empty);
            content = content.TrimStart(new char[] { '\r', '\n' });

            File.WriteAllText(path, content);
        }
    }
}