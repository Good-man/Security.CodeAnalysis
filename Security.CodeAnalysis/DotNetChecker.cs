using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Security.CodeAnalysis.CSharp;

namespace Security.CodeAnalysis
{
    public class DotNetChecker : CodeChecker
    {
        private readonly FileInfo _fileInfo;

        public DotNetChecker(string fileName)
        {
            _fileInfo = new FileInfo(fileName);
        }

        public override void Check()
        {
            if (!_fileInfo.Exists)
                throw new FileNotFoundException("Project or Solution file not found", _fileInfo.FullName);

            // start Roslyn workspace
            // Note: This requires the newer build tools to be installed on the machine
            var workspace = MSBuildWorkspace.Create();

            switch (_fileInfo.Extension)
            {
                case ".sln":            // open solution we want to analyze
                    var solutionToAnalyze =
                        workspace.OpenSolutionAsync(_fileInfo.FullName).Result;
                    CheckSolution(solutionToAnalyze);
                    break;
                case ".csproj":
                    var projectToAnalyze = workspace.OpenProjectAsync(_fileInfo.FullName).Result;
                    CheckProject(projectToAnalyze);
                    break;
                default:
                    throw new InvalidOperationException("Expected a Project or Solution file");
                    break;
            }

            Tracer.Flush();
        }

        private void CheckSolution(Solution solutionToAnalyze)
        {
            foreach (var project in solutionToAnalyze.Projects)
            {
                CheckProject(project);
            }
        }

        private void CheckProject(Project project)
        {
            Tracer.TraceEvent(TraceEventType.Start, 0, "{0} ({1})", project.Name, project.Language);

            if (project.Language != "C#")
                return;

            // get the project's compilation
            // compilation contains all the types of the 
            // project and the projects referenced by 
            // our project. 
            var compilation = project.GetCompilationAsync().Result;

            Trace.WriteLine(compilation.SyntaxTrees.Count());
            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                var checker = new CSharpSyntaxTreeChecker(syntaxTree);
                checker.CodeIssueFound += (sender, args) => OnCodeIssueFound(args);
                checker.Check();
            }

            Tracer.TraceEvent(TraceEventType.Stop, 0, "{0} ({1})", project.Name, project.Language);
        }
    }
}