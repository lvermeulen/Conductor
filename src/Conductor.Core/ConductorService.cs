using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Executor;

namespace Conductor.Core
{
    public class ConductorService : IConductorService
    {
        public ConcurrentDictionary<string, BuildChannel> Channels { get; } = new ConcurrentDictionary<string, BuildChannel>();
        public IList<ExpressionDetailFile> ExpressionDetailFiles { get; }
        public IList<ExpressionFile> ExpressionFiles { get; }

        private readonly SubscriptionsExecutor _subscriptionsExecutor;

        internal ConductorService(IList<ExpressionDetailFile> expressionDetailFiles, IList<ExpressionFile> expressionFiles)
        {
            ExpressionDetailFiles = expressionDetailFiles;
            ExpressionFiles = expressionFiles;
            _subscriptionsExecutor = new SubscriptionsExecutor(this);
        }

        public Task<BuildChannel> AddChannelAsync(string name, ClassificationType classificationType, string repositoryUrl, string branchName)
        {
            var channel = new BuildChannel(name, classificationType, repositoryUrl, branchName);
            var newChannel = Channels.AddOrUpdate(name, channelName => channel, (channelName, oldChannel) => channel);

            return Task.FromResult(newChannel);
        }

        public async Task RemoveChannelAsync(string name)
        {
            var channel = await FindChannelByNameAsync(name);
            Channels.TryRemove(channel.Name, out var _);
        }

        public Task<BuildChannel> FindChannelByNameAsync(string channelName) => Task.FromResult(Channels.FirstOrDefault(x => x.Value.Name.Equals(channelName, StringComparison.InvariantCultureIgnoreCase)).Value);

        public async Task<bool> AddOrUpdateBuildChannelAsync(BuildInfo buildInfo, CancellationToken cancellationToken = default)
        {
	        if (buildInfo is null || string.IsNullOrWhiteSpace(buildInfo.ChannelName))
	        {
		        throw new ArgumentNullException(nameof(buildInfo));
	        }

	        string channelName = buildInfo.ChannelName;
	        var channel = await FindChannelByNameAsync(channelName);
	        if (channel is not null)
	        {
		        bool result = await channel.AddOrUpdateBuildAsync(buildInfo);
		        if (result)
		        {
			        foreach (var buildChannel in Channels)
			        {
				        foreach (var subscription in buildChannel.Value.Subscriptions)
				        {
					        await _subscriptionsExecutor.ExecuteSubscriptionAsync(buildInfo, subscription, cancellationToken);
				        }
			        }
		        }
	        }

	        return false;
        }

        public async Task<IEnumerable<Dependency>> DownloadAssetsAsync(string url, CancellationToken cancellationToken = default)
        {
	        return Enumerable.Empty<Dependency>();
        }
    }
}
