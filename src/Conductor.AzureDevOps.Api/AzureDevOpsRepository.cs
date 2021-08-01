using System.Text.Json.Serialization;

namespace Conductor.AzureDevOps.Api
{
	public class AzureDevOpsRepository
	{
		public string Name { get; }
		public string DefaultBranch { get; }
		public string RemoteUrl { get; }

		[JsonConstructor]
		public AzureDevOpsRepository(string name, string defaultBranch, string remoteUrl)
		{
			Name = name;
			DefaultBranch = defaultBranch;
			RemoteUrl = remoteUrl;
		}

		public override string ToString() => $"{Name}, {DefaultBranch}, {RemoteUrl}";
	}
}
