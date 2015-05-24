using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Security.CodeAnalysis.CSharp
{
    public class CSharpFileChecker : FileChecker
    {

        public CSharpFileChecker(FileInfo fileInfo)
            : base(fileInfo)
        {
        }

        public override void Check()
        {
            if (!FileInfo.Exists) throw new FileNotFoundException();

            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(FileInfo.FullName));

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
                            var isController = classDeclarationSyntax.BaseList.Types.Any(b => b.Type.ToString() == "Controller");
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