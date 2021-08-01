using System.Collections.Generic;
using Newtonsoft.Json;

namespace Conductor.AzureDevOps.Api.Models
{
	public class GitCommitRef
	{
		[JsonProperty("_links")]
		public ReferenceLinks Links { get; set; }
		public GitUserDate Author { get; set; }
		public ChangeCountDictionary ChangeCounts { get; set; }
		public IEnumerable<GitChange> Changes { get; set; }
		public string Comment { get; set; }
		public bool CommentTruncated { get; set; }
		public string CommitId { get; set; }
		public GitUserDate Committer { get; set; }
		public IEnumerable<string> Parents { get; set; }
		public GitPushRef Push { get; set; }
		public string RemoteUrl { get; set; }
		public IEnumerable<GitStatus> Statuses { get; set; }
		public string Url { get; set; }
		public IEnumerable<ResourceRef> WorkItems { get; set; }
	}
}
