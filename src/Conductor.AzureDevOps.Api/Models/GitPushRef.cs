using Newtonsoft.Json;

namespace Conductor.AzureDevOps.Api.Models
{
	public class GitPushRef
	{
		[JsonProperty("_links")]
		public ReferenceLinks Links { get; set; }

        public string Date { get; set; }
		public int PushId { get; set; }
		public IdentityRef PushedBy { get; set; }
		public string Url { get; set; }
	}
}
