namespace Conductor.AzureDevOps.Api.Models
{
	public enum PullRequestAsyncStatus
	{
		Conflicts,
		Failure,
		NotSet,
		Queued,
		RejectedByPolicy,
		Succeeded
	}
}
