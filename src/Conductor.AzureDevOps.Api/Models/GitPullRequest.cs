using System.Collections.Generic;
using Newtonsoft.Json;

namespace Conductor.AzureDevOps.Api.Models
{
	public class GitPullRequest
	{
		[JsonProperty("_links")]
		public ReferenceLinks Links { get; set; }
		public string ArtifactId { get; set; }
		public IdentityRef AutoCompleteSetBy { get; set; }
		public IdentityRef ClosedBy { get; set; }
		public string ClosedDate { get; set; }
		public int CodeReviewId { get; set; }
		public IEnumerable<GitCommitRef> Commits { get; set; }
		public GitPullRequestCompletionOptions CompletionOptions { get; set; }
		public string CompletionQueueTime { get; set; }
		public IdentityRef CreatedBy { get; set; }
		public string CreationDate { get; set; }
		public string Description { get; set; }
		public GitForkRef ForkSource { get; set; }
		public bool IsDraft { get; set; }
		public IEnumerable<WebApiTagDefinition> Labels { get; set; }
		public GitCommitRef LastMergeCommit { get; set; }
		public GitCommitRef LastMergeSourceCommit { get; set; }
		public GitCommitRef LastMergeTargetCommit { get; set; }
		public string MergeFailureMessage { get; set; }
		public PullRequestMergeFailureType MergeFailureType { get; set; }
		public string MergeId { get; set; }
		public GitPullRequestMergeOptions MergeOptions { get; set; }
		public PullRequestAsyncStatus MergeStatus { get; set; }
		public int PullRequestId { get; set; }
		public string RemoteUrl { get; set; }
		public GitRepository Repository { get; set; }
		public IEnumerable<IdentityRefWithVote> Reviewers { get; set; }
		public string SourceRefName { get; set; }
		public PullRequestStatus Status { get; set; }
		public bool SupportsIterations { get; set; }
		public string TargetRefName { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public IEnumerable<ResourceRef> WorkItemRefs { get; set; }
	}
}
