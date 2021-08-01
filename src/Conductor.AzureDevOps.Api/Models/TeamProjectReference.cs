namespace Conductor.AzureDevOps.Api.Models
{
	public class TeamProjectReference
	{
		public string Abbreviation { get; set; }
		public string DefaultTeamImageUrl { get; set; }
		public string Description { get; set; }
		public string Id { get; set; }
		public string LastUpdateTime { get; set; }
		public string Name { get; set; }
		public int? Revision { get; set; }
		public ProjectState State { get; set; }
		public string Url { get; set; }
		public ProjectVisibility Visibility { get; set; }
	}
}
