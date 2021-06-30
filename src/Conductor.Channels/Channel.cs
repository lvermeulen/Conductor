using System;
using Conductor.Subscriptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conductor.Channels
{
	public record Channel
	{
		public string Name { get; }
		public ClassificationType Classification { get; }
		public string RepositoryUrl { get; }
		public string BranchName { get; }

		public IList<Subscription> Subscriptions { get; } = new List<Subscription>();

		public Channel(string name, ClassificationType classification, string repositoryUrl, string branchName)
		{
			Name = name;
			Classification = classification;
			RepositoryUrl = repositoryUrl;
			BranchName = branchName;
		}

		private Task<Subscription> AddSubscriptionAsync(Subscription subscription)
		{
			Subscriptions.Add(subscription);

			return Task.FromResult(subscription);
		}

		public async Task<Subscription> AddSubscriptionAsync(string sourceRepositoryUrl, string targetRepositoryUrl, string targetBranchName, UpdateFrequency updateFrequency, IEnumerable<string> policies)
		{
			var subscription = new Subscription(Name, sourceRepositoryUrl, targetRepositoryUrl, targetBranchName, updateFrequency, policies) { Id = Guid.NewGuid() };
			await AddSubscriptionAsync(subscription);

			return subscription;
		}

		public Task<Subscription> EditSubscriptionAsync(Guid id, string sourceRepositoryUrl, string targetRepositoryUrl, string targetBranchName, UpdateFrequency updateFrequency, IEnumerable<string> policies)
		{
			var subscription = Subscriptions.FirstOrDefault(x => x.Id == id);
			if (subscription is not null)
			{
				// remove
				Subscriptions.Remove(subscription);

				// add
				subscription = subscription with
				{
					SourceRepositoryUrl = sourceRepositoryUrl,
					TargetRepositoryUrl = targetRepositoryUrl,
					TargetBranchName = targetBranchName,
					UpdateFrequency = updateFrequency,
					Policies = policies
				};
				Subscriptions.Add(subscription);
			}

			return Task.FromResult(subscription);
		}

		public Task RemoveSubscriptionAsync(Subscription subscription)
		{
			Subscriptions.Remove(subscription);
			return Task.CompletedTask;
		}
	}
}
