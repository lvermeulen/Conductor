namespace Conductor.Abstractions
{
	public record Dependency(string Name, string Version, string Sha, bool? Pinned = false, string Expression = "")
	{
		public DependencyType DependencyType { get; set; }
		public string Url { get; set; }
		public string Uri { get; set; }
	};
}
