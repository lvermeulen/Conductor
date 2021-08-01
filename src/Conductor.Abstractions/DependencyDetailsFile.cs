namespace Conductor.Abstractions
{
    public class DependencyDetailsFile
    {
        public string FileName { get; }
        public string SubPath { get; }
        public DependencyFileType DependencyFileType { get; }

        public DependencyDetailsFile(string fileName, string subPath, DependencyFileType dependencyFileType)
        {
            FileName = fileName;
            SubPath = subPath;
            DependencyFileType = dependencyFileType;
        }
    }
}
