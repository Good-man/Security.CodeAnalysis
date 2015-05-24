using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Security.CodeAnalysis.CSharp
{
    public class ControllerChecker : CodeChecker
    {
        private readonly ClassDeclarationSyntax _node;

        public ControllerChecker(ClassDeclarationSyntax node)
        {
            _node = node;
        }

        public override void Check()
        {
            foreach (var m in _node.Members)
            {
                if (m is PropertyDeclarationSyntax)
                {
                    Tracer.TraceEvent(TraceEventType.Information, 0, "Property::" + m.GetText());
                }
                var method = m as MethodDeclarationSyntax;
                if (method != null)
                {
                    if (method.Modifiers.Any(modifier => modifier.Text == "public"))
                    {
                        var checker = new ActionMethodChecker(method);
                        checker.CodeIssueFound += (sender, args) => { OnCodeIssueFound(args); };
                        checker.Check();
                    }
                    else
                    {
                        Tracer.TraceEvent(TraceEventType.Information, 0, "Method::" + method.GetText());
                    }
                }
                if (m is FieldDeclarationSyntax)
                {
                    Tracer.TraceEvent(TraceEventType.Information, 0, "Variable::" + m.GetText());
                }
            }
        }
    }
}
