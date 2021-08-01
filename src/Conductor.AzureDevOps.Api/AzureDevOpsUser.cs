using System;

namespace Conductor.AzureDevOps.Api
{
	public readonly struct AzureDevOpsUser
	{
		public Guid Id { get; }
		public string Name { get; }
		public string EmailAddress { get; }

		public AzureDevOpsUser(Guid id, string name, string emailAddress)
		{
			Id = id;
			Name = name;
			EmailAddress = emailAddress;
		}
	}
}
