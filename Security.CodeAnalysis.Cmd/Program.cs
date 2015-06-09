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

            //var pathToSolution = args[0];
            //if (!File.Exists(pathToSolution))
            //    throw new FileNotFoundException("File not found", pathToSolution);


            //// start Roslyn workspace
            //// Note: This requires the newer build tools to be installed on the machine
            //var workspace = MSBuildWorkspace.Create();

            //// open solution we want to analyze
            //var solutionToAnalyze =
            //    workspace.OpenSolutionAsync(pathToSolution).Result;

            //foreach (var project in solutionToAnalyze.Projects)
            //{
            //    CodeChecker.Tracer.TraceEvent(TraceEventType.Start, 0, "{0} ({1})", project.Name, project.Language);

            //    if (project.Language != "C#")
            //        continue;

            //    // get the project's compilation
            //    // compilation contains all the types of the 
            //    // project and the projects referenced by 
            //    // our project. 
            //    var compilation = project.GetCompilationAsync().Result;

            //    Trace.WriteLine(compilation.SyntaxTrees.Count());
            //    foreach (var syntaxTree in compilation.SyntaxTrees)
            //    {
            //        var checker = new CSharpSyntaxTreeChecker(syntaxTree);
            //        checker.CodeIssueFound += checker_CodeIssueFound;
            //        checker.Check();
            //    }

            //    CodeChecker.Tracer.TraceEvent(TraceEventType.Stop, 0, "{0} ({1})", project.Name, project.Language);
            //}

            //CodeChecker.Tracer.Flush();
        }

        private static void checker_CodeIssueFound(object sender, CodeIssueFoundEventArgs e)
        {
            CodeChecker.Tracer.TraceEvent(TraceEventType.Warning, 0, e.ToString());
        }
    }
}