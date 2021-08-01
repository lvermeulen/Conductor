using System.Collections.Generic;

namespace Conductor.AzureDevOps.Api.Models
{
	public class PagedGraphMemberList
	{
		public IEnumerable<UserEntitlement> Members { get; set; }
	}
}
