using System.Collections.Generic;

namespace Conductor.AzureDevOps.Api.Models
{
	public class GitPullRequestCompletionOptions
	{
		public IEnumerable<int> AutoCompleteIgnoreConfigIds { get; set; }
		public bool BypassPolicy { get; set; }
		public string BypassReason { get; set; }
		public bool DeleteSourceBranch { get; set; }
		public string MergeCommitMessage { get; set; }
		public GitPullRequestMergeStrategy MergeStrategy { get; set; }
		public bool SquashMerge { get; set; }
		public bool TransitionWorkItems { get; set; }
		public bool TriggeredByAutoComplete { get; set; }
	}
}
