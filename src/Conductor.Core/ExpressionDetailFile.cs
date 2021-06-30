using Conductor.Abstractions;

namespace Conductor.Core
{
	public class ExpressionDetailFile
	{
		public string FileName { get; }
		public string SubPath { get; }
		public DependencyFileType DependencyFileType { get; }

		public ExpressionDetailFile(string fileName, string subPath, DependencyFileType dependencyFileType)
		{
			FileName = fileName;
			SubPath = subPath;
			DependencyFileType = dependencyFileType;
		}
	}
}
