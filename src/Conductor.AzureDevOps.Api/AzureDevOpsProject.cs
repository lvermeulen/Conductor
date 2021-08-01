namespace Conductor.AzureDevOps.Api
{
	public readonly struct AzureDevOpsProject
	{
		public string Id { get; }
		public string Name { get; }

		public AzureDevOpsProject(string id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
