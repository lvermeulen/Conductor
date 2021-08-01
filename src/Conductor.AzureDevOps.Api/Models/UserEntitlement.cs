namespace Conductor.AzureDevOps.Api.Models
{
	public class UserEntitlement
	{
		public object AccessLevel { get; set; }
		public object DateCreated { get; set; }
		public object GroupAssignments { get; set; }
		public string Id { get; set; }
		public object LastAccessedDate { get; set; }
		public object ProjectEntitlements { get; set; }
		public GraphUser User { get; set; }
	}
}
