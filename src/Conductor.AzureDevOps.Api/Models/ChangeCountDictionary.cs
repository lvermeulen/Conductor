namespace Conductor.AzureDevOps.Api.Models
{
	public class ChangeCountDictionary
	{
		public int ChangeId { get; set; }
		public VersionControlChangeType ChangeType { get; set; }
		public string Item { get; set; }
		public ItemContent NewContent { get; set; }
		public GitTemplate NewContentTemplate { get; set; }
		public string OriginalPath { get; set; }
		public string SourceServerItem { get; set; }
		public string Url { get; set; }
	}
}
