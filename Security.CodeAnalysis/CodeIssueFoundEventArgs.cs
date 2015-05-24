namespace Security.CodeAnalysis
{
    public class CodeIssueFoundEventArgs
    {
        public CodeIssue CodeIssue { get; set; }

        public override string ToString()
        {
            return CodeIssue.ToString();
        }
    }
}