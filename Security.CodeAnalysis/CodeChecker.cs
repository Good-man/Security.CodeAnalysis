using System;
using System.Diagnostics;

namespace Security.CodeAnalysis
{
    public abstract class CodeChecker
    {
        public static TraceSource Tracer = new TraceSource("CodeChecker");
        public virtual event EventHandler<CodeIssueFoundEventArgs> CodeIssueFound;
        public abstract void Check();

        protected void OnCodeIssueFound(CodeIssueFoundEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            var handler = CodeIssueFound;
            if (handler != null)
                handler(this, e);
        }

        protected void OnCodeIssueFound(string title, string description, string fileName,
            SeverityLevel severityLevel = SeverityLevel.Standard, string codeLine = "", int lineNumber = 0)
        {
            OnCodeIssueFound(new CodeIssueFoundEventArgs
            {
                CodeIssue = new CodeIssue
                {
                    Title = title,
                    Description = description,
                    FileName = fileName,
                    SeverityLevel = severityLevel,
                    CodeLine = codeLine,
                    LineNumber = lineNumber
                }
            });
        }
    }
}