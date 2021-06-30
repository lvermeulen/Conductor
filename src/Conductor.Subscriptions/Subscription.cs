using System;
using System.Collections.Generic;

namespace Conductor.Subscriptions
{
	public record Subscription(string ChannelName, string SourceRepositoryUrl, string TargetRepositoryUrl, string TargetBranchName, UpdateFrequency UpdateFrequency, IEnumerable<string> Policies)
	{
		public Guid Id { get; init; }
	}
}
