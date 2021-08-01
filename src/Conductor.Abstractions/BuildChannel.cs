using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Conductor.Abstractions
{
    public class BuildChannel
    {
        public IConductorService Conductor { get; }
        public string Name { get; }
        public ClassificationType Classification { get; }
        public string RepositoryUrl { get; }
        public string BranchName { get; }

        private IDictionary<string, BuildInfo> Builds { get; } = new Dictionary<string, BuildInfo>();
        public IList<Subscription> Subscriptions { get; } = new List<Subscription>();

        public BuildChannel(IConductorService conductor, string name, ClassificationType classification, string repositoryUrl, string branchName)
        {
	        Conductor = conductor;
            Name = name;
            Classification = classification;
            RepositoryUrl = repositoryUrl;
            BranchName = branchName;
        }

        public Task<IEnumerable<BuildInfo>> GetBuildsAsync() => Task.FromResult(Builds.Values.AsEnumerable());

        public Task<BuildInfo> GetBuildAsync(string buildName, CancellationToken cancellationToken)
        {
	        var result = Builds.ContainsKey(buildName)
                ? Builds[buildName]
                : default;

	        return Task.FromResult(result);
        }

        public Task<bool> AddOrUpdateBuildAsync(BuildInfo buildInfo, CancellationToken cancellationToken)
        {
	        if (buildInfo is null)
	        {
		        throw new ArgumentNullException(nameof(buildInfo));
	        }

	        Builds[buildInfo.ChannelName] = buildInfo;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveBuildAsync(string buildName, CancellationToken cancellationToken)
        {
	        Builds.Remove(buildName);
	        return Task.FromResult(true);
        }

        private Task<Subscription> AddSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken)
        {
            Subscriptions.Add(subscription);

            return Task.FromResult(subscription);
        }

        public async Task<Subscription> AddSubscriptionAsync(string sourceRepositoryUrl, string targetRepositoryUrl, string targetBranchName, UpdateFrequency updateFrequency, IEnumerable<string> policies, CancellationToken cancellationToken)
        {
            var subscription = new Subscription(Name, sourceRepositoryUrl, targetRepositoryUrl, targetBranchName, updateFrequency, policies) { Id = Guid.NewGuid() };
            await AddSubscriptionAsync(subscription, cancellationToken);

            return subscription;
        }

        public Task<Subscription> EditSubscriptionAsync(Guid id, string sourceRepositoryUrl, string targetRepositoryUrl, string targetBranchName, UpdateFrequency updateFrequency, IEnumerable<string> policies, CancellationToken cancellationToken)
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

        public Task RemoveSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken)
        {
            Subscriptions.Remove(subscription);
            return Task.CompletedTask;
        }
    }
}
