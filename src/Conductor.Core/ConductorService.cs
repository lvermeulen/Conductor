using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;

namespace Conductor.Core
{
    public class ConductorService : IConductorService
    {
        public ConcurrentDictionary<string, BuildChannel> Channels { get; } = new ConcurrentDictionary<string, BuildChannel>();
        public IList<DependencyDetailsFile> DependencyDetailsFiles { get; }
        public IList<ExpressionFile> ExpressionFiles { get; }

        public JsonSerializerType JsonSerializerType { get; }
        private readonly SubscriptionsExecutor _subscriptionsExecutor;
        private readonly BuildsExecutor _buildsExecutor;

        internal ConductorService(IList<DependencyDetailsFile> dependencyDetailsFiles, IList<ExpressionFile> expressionFiles, JsonSerializerType jsonSerializerType)
        {
	        DependencyDetailsFiles = dependencyDetailsFiles;
            ExpressionFiles = expressionFiles;
            JsonSerializerType = jsonSerializerType;

            _subscriptionsExecutor = new SubscriptionsExecutor(this);
            _buildsExecutor = new BuildsExecutor(this);
        }

        public Task<BuildChannel> AddBuildChannelAsync(string name, ClassificationType classificationType, string repositoryUrl, string branchName, CancellationToken cancellationToken = default)
        {
            var channel = new BuildChannel(this, name, classificationType, repositoryUrl, branchName);
            var newChannel = Channels.AddOrUpdate(name, channelName => channel, (channelName, oldChannel) => channel);

            return Task.FromResult(newChannel);
        }

        public async Task RemoveBuildChannelAsync(string name, CancellationToken cancellationToken = default)
        {
            var channel = await FindBuildChannelByNameAsync(name, cancellationToken);
            Channels.TryRemove(channel.Name, out var _);
        }

        public Task<BuildChannel> FindBuildChannelByNameAsync(string channelName, CancellationToken cancellationToken = default) => Task.FromResult(Channels.FirstOrDefault(x => x.Value.Name.Equals(channelName, StringComparison.InvariantCultureIgnoreCase)).Value);

        public async Task<Subscription> FindSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default)
        {
	        var channel = await FindBuildChannelByNameAsync(subscription.ChannelName, cancellationToken);
	        int index = channel.Subscriptions.IndexOf(subscription);
	        return channel.Subscriptions.ElementAt(index);
        }

        public async Task<bool> AddOrUpdateBuildChannelAsync(BuildInfo buildInfo, string repositoryPath, CancellationToken cancellationToken = default)
        {
	        if (buildInfo is null || string.IsNullOrWhiteSpace(buildInfo.ChannelName))
	        {
		        throw new ArgumentNullException(nameof(buildInfo));
	        }

	        string channelName = buildInfo.ChannelName;
	        var channel = await FindBuildChannelByNameAsync(channelName, cancellationToken);
	        if (channel is not null)
	        {
		        bool result = await channel.AddOrUpdateBuildAsync(buildInfo, cancellationToken);
		        if (result)
		        {
			        foreach (var buildChannel in Channels)
			        {
				        foreach (var subscription in buildChannel.Value.Subscriptions)
				        {
					        await _subscriptionsExecutor.ExecuteSubscriptionAsync(buildInfo, subscription, repositoryPath, cancellationToken);
				        }
			        }
		        }

		        return result;
	        }

	        return false;
        }
    }
}
