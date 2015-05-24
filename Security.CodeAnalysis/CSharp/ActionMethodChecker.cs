using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Security.CodeAnalysis.CSharp
{
    public class ActionMethodChecker : CodeChecker
    {
        private readonly MethodDeclarationSyntax _node;

        public ActionMethodChecker(MethodDeclarationSyntax node)
        {
            _node = node;
        }

        public override void Check()
        {
            var allAttributes = _node.AttributeLists.SelectMany(al => al.Attributes).ToArray();
            var isHttpPost = allAttributes.Any(a => a.ToString().StartsWith("HttpPost"));
            var validatesAntiForgery = allAttributes.Any(a => a.ToString().StartsWith("ValidateAntiForgery"));
            if (isHttpPost && !validatesAntiForgery)
            {
                OnCodeIssueFound("Possible CSRF Vulnerability", "The following action method accepts POST requests, but does not appear to have Anti-Forgery measures in place"
                    , "", SeverityLevel.High, _node.GetText().ToString());
            }
            else
            {
                Tracer.TraceEvent(TraceEventType.Information, 0, "Action Method::{0}", _node.GetText());
            }
        }
    }
}