using System.Diagnostics;
using System.IO;
using Fclp;

namespace Security.CodeAnalysis.Cmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var p = new FluentCommandLineParser();

            p.Setup<string>('t', "target")
                .Callback(filename => FileName = filename)
                .Required()
                .WithDescription("Full path to the target project, solution or file.");

            p.Parse(args);

            if (!File.Exists(FileName))
                throw new FileNotFoundException("File not found", FileName);

            var checker = new DotNetChecker(FileName);
            checker.CodeIssueFound += checker_CodeIssueFound;
            checker.Check();
        }

        public static string FileName { get; set; }

        private static void checker_CodeIssueFound(object sender, CodeIssueFoundEventArgs e)
        {
            CodeChecker.Tracer.TraceEvent(TraceEventType.Warning, 0, e.ToString());
        }
    }
}