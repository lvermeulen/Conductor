using Newtonsoft.Json;

namespace Conductor.AzureDevOps.Api.Models
{
	public class IdentityRef
	{
		[JsonProperty("_links")]
		public ReferenceLinks Links { get; set; }
		public string Descriptor { get; set; }
		public string DirectoryAlias { get; set; }
		public string DisplayName { get; set; }
		public string Id { get; set; }
		public string ImageUrl { get; set; }
		public bool Inactive { get; set; }
		public bool IsAadIdentity { get; set; }
		public bool IsContainer { get; set; }
		public bool IsDeletedInOrigin { get; set; }
		public string ProfileUrl { get; set; }
		public string UniqueName { get; set; }
		public string Url { get; set; }
	}
}
