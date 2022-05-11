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
    using System.Threading.Tasks;
#endif

#if NETFRAMEWORK
    public abstract class ViewBase<T> : TemplateBase<T>
#else
    public abstract class ViewBase<T>
#endif
    {
#if NETFRAMEWORK
        public new T Model { get; set; }
#else
        public T Model { get; set; }
#endif

        public abstract string PageName { get; }

        public abstract byte[] Template { get; }

        public string ViewFilePath
        {
            get
            {
                return Path.Combine(HtmlHelp.HelpContentSourcePath, this.TemplateName + ".cshtml");
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
            string templateText = LoadTemplate();

            // unicode result
            string content = Engine.Razor.RunCompile(templateText, GetStringSha256Hash(templateText), null, this.Model);
#else
            IRazorEngine razorEngine = new RazorEngine();
            string templateText = LoadTemplate();

            templateText = Regex.Replace(templateText, "@model .+", "");

            IRazorEngineCompiledTemplate<HtmlSafeTemplate<T>> template = razorEngine.Compile<HtmlSafeTemplate<T>>(templateText);

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

        private string LoadTemplate()
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
        }

        private static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }

#if !NETFRAMEWORK
    public class HtmlSafeTemplate<T> : RazorEngineTemplateBase<T>
    {
        class RawContent
        {
            public object Value { get; set; }

            public RawContent(object value)
            {
                Value = value;
            }
        }

        public object Raw(object value)
        {
            return new RawContent(value);
        }

        public override Task WriteAsync(object obj = null)
        {
            object value = obj is RawContent rawContent
                ? rawContent.Value
                : System.Web.HttpUtility.HtmlEncode(obj);

            return base.WriteAsync(value);
        }

        public override Task WriteAttributeValueAsync(string prefix, int prefixOffset, object value, int valueOffset, int valueLength, bool isLiteral)
        {
            value = value is RawContent rawContent
                ? rawContent.Value
                : System.Web.HttpUtility.HtmlAttributeEncode(value?.ToString());

            return base.WriteAttributeValueAsync(prefix, prefixOffset, value, valueOffset, valueLength, isLiteral);
        }
    }
#endif
}