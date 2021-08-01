namespace Conductor.AzureDevOps.Api.Models
{
	public enum GitPullRequestMergeStrategy
	{
		NoFastForward,
		Rebase,
		RebaseMerge,
		Squash
	}
}
