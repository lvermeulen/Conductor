using System.Collections.Generic;
using Newtonsoft.Json;

namespace Conductor.AzureDevOps.Api.Models
{
	public class GitRepository : GitRepositoryBase
	{
		[JsonProperty("_links")]
		public ReferenceLinks Links { get; set; }
		public string DefaultBranch { get; set; }
		public GitRepositoryRef ParentRepository { get; set; }
		public TeamProjectReference Project { get; set; }
		public int? Size { get; set; }
		public IEnumerable<string> ValidRemoteUrls { get; set; }
		public string WebUrl { get; set; }
	}
}
