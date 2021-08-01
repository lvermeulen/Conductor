namespace Conductor.AzureDevOps.Api.Models
{
	public abstract class GitRepositoryBase
	{
		public string Id { get; set; }
		public bool? IsFork { get; set; }
		public string Name { get; set; }
		public string RemoteUrl { get; set; }
		public string SshUrl { get; set; }
		public string Url { get; set; }
	}
}
