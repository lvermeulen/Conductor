namespace Conductor.Abstractions
{
    public class ExpressionFile
    {
        public string FileName { get; }
        public string SubPath { get; }
        public ExpressionFileType ExpressionFileType { get; }

        public ExpressionFile(string fileName, string subPath, ExpressionFileType expressionFileType)
        {
            FileName = fileName;
            SubPath = subPath;
            ExpressionFileType = expressionFileType;
        }
    }
}
