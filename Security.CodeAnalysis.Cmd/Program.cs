using System.Diagnostics;
using System.IO;

namespace Security.CodeAnalysis.Cmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fileName = args[0];
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not found", fileName);

            var checker = new DotNetChecker(fileName);
            checker.CodeIssueFound += checker_CodeIssueFound;
            checker.Check();
        }

        private static void checker_CodeIssueFound(object sender, CodeIssueFoundEventArgs e)
        {
            CodeChecker.Tracer.TraceEvent(TraceEventType.Warning, 0, e.ToString());
        }
    }
}