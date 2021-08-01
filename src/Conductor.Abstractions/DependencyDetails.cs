namespace Conductor.Abstractions
{
	public record DependencyDetails(string FileName, string Version, string Sha, DependencyType DependencyType)
	{
		public bool Pinned { get; init; }
		public string Expression { get; init; }
	}
}
