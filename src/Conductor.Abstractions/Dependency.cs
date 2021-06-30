namespace Conductor.Abstractions
{
	public class Dependency
	{
		public string Name { get; set; }
		public string Version { get; set; }
		public string Url { get; set; }
		public string Uri { get; set; }
		public string Sha { get; set; }
		public DependencyType DependencyType { get; set; }
		public bool? Pinned { get; set; }
		public string Expression { get; set; }

		public Dependency(string name, string version, string url, string sha, DependencyType dependencyType, bool? pinned = false, string expression = "")
		{
			Name = name;
			Version = version;
			Url = url;
			Sha = sha;
			DependencyType = dependencyType;
			Pinned = pinned;
			Expression = expression;
		}
	}
}
