namespace Security.CodeAnalysis
{
    public class CodeIssue
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public SeverityLevel SeverityLevel { get; set; }
        public string FileName { get; set; }
        public string CodeLine { get; set; }
        public int LineNumber { get; set; }

        public CodeIssue()
        {
            SeverityLevel = SeverityLevel.Standard;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}\n{4}", Title, Description, SeverityLevel, FileName, CodeLine);
        }
    }
}