using Newtonsoft.Json;

namespace Conductor.AzureDevOps.Api.Models
{
	public class GitStatus
	{
		[JsonProperty("_links")]
		public ReferenceLinks Links { get; set; }
		public GitStatusContext Context { get; set; }
		public IdentityRef CreatedBy { get; set; }
		public string CreationDate { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public GitStatusState State { get; set; }
		public string TargetUrl { get; set; }
		public string UpdatedDate { get; set; }
	}
}
