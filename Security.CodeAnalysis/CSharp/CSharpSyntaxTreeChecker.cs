using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Security.CodeAnalysis.CSharp
{
    public class CSharpSyntaxTreeChecker : CodeChecker
    {
        private readonly SyntaxTree _syntaxTree;

        public CSharpSyntaxTreeChecker(SyntaxTree syntaxTree)
        {
            _syntaxTree = syntaxTree;
        }

        public override void Check()
        {
            var tree = _syntaxTree;

            var root = (CompilationUnitSyntax)tree.GetRoot();

            foreach (var n in root.Members)
            {
                if (n is NamespaceDeclarationSyntax)
                {
                    foreach (var t in (n as NamespaceDeclarationSyntax).Members)
                    {
                        if (t is ClassDeclarationSyntax)
                        {
                            var classDeclarationSyntax = (t as ClassDeclarationSyntax);

                            var isController = classDeclarationSyntax.BaseList != null
                                && classDeclarationSyntax.BaseList.Types.Any(b => b.Type.ToString() == "Controller");
                            if (isController)
                            {
                                var checker = new ControllerChecker(classDeclarationSyntax);
                                checker.CodeIssueFound += (sender, args) => { OnCodeIssueFound(args); };
                                checker.Check();
                            }
                        }
                    }
                }
            }
        }
    }
}