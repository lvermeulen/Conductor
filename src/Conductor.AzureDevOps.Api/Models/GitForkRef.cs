using System.Collections.Generic;
using Newtonsoft.Json;

namespace Conductor.AzureDevOps.Api.Models
{
	public class GitForkRef
	{
		[JsonProperty("_links")]
		public ReferenceLinks Links { get; set; }

        public IdentityRef Creator { get; set; }
		public bool IsLocked { get; set; }
		public IdentityRef IsLockedBy { get; set; }
		public string Name { get; set; }
		public string ObjectId { get; set; }
		public string PeeledObjectId { get; set; }
		public GitRepository Repository { get; set; }
		public IEnumerable<GitStatus> Statuses { get; set; }
		public string Url { get; set; }
	}
}
