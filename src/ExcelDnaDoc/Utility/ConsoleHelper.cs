namespace ExcelDnaDoc
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public static class ConsoleHelper
    {
        public static void RunBatch(string batchPath)
        {
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = batchPath;
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
        }

        public static void RunCommand(string command, string[] parameters, string workingDirectory = null)
        {
            var process = new Process();
    
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = string.Concat(parameters.Select(p => ToParam(p)));
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            if (workingDirectory != null) 
            {
                process.StartInfo.WorkingDirectory = workingDirectory;            
            }

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            Console.Write(output);

            process.WaitForExit();
        }

        public static string QuoteIfNeeded(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            else if (s.Contains(" "))
            {
                return Quote(s);
            }
            else
            {
                return s;
            }
        }

        public static string Quote (string s)
        {
            return "\"" + s + "\"";
        }

        public static string ToParam (string s)
        {
            return " " + QuoteIfNeeded(s);
        }
    }
}
