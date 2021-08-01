namespace Conductor.AzureDevOps.Api.Models
{
	public class GitRepositoryRef : GitRepositoryBase
	{
		public TeamProjectCollectionReference Collection { get; set; }
		public TeamProjectReference Project { get; set; }
	}
}
