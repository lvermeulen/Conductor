namespace Conductor.AzureDevOps.Api.Models
{
	public class GitPullRequestMergeOptions
	{
		public bool ConflictAuthorshipCommits { get; set; }
		public bool DetectRenameFalsePositives { get; set; }
		public bool DisableRenames { get; set; }
	}
}
