namespace Conductor.AzureDevOps.Api.Models
{
	public enum Votes
	{
		Approved = 10,
		ApprovedWithSuggestions = 5,
		NoVote = 0,
		WaitingForAuthor = -5,
		Rejected = -10
	}
}
