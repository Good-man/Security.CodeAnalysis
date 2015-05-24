using System.IO;
using System.Linq;
using Security.CodeAnalysis.CSharp;

namespace Security.CodeAnalysis
{
    public class FileChecker : CodeChecker
    {
        internal protected FileChecker(string fileInfo)
            : this(new FileInfo(fileInfo))
        {

        }

        internal protected FileChecker(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public FileInfo FileInfo { get; set; }
        

        protected string GetCodeLine(int lineNumber)
        {
            return File.ReadLines(FileInfo.FullName).Skip(lineNumber - 1).Take(1).Single();
        }


        public static FileChecker Create(string fileName)
        {
            return Create(new FileInfo(fileName));
        }

        public static FileChecker Create(FileInfo fileInfo)
        {
            if (fileInfo.Extension == ".cs")
            {
                return new CSharpFileChecker(fileInfo);
            }
            return new FileChecker(fileInfo);
        }

        public override void Check()
        {
            OnCodeIssueFound("File Check Not Implemented", "Checking for this type of file has not been implemented", FileInfo.FullName, SeverityLevel.Info);
        }
    }
}