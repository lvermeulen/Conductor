using System.Collections.Generic;

namespace Conductor.AzureDevOps.Api.Models
{
	public class IdentityRefWithVote : IdentityRef
	{
		public bool HasDeclined { get; set; }
		public bool IsFlagged { get; set; }
		public bool IsRequired { get; set; }
		public string ReviewerUrl { get; set; }
		public Votes Vote { get; set; }
		public IEnumerable<IdentityRefWithVote> VotedFor { get; set; }
	}
}
