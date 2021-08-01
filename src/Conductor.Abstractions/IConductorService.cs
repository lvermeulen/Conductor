using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Conductor.Abstractions
{
	public interface IConductorService
	{
		ConcurrentDictionary<string, BuildChannel> Channels { get; }
		IList<DependencyDetailsFile> DependencyDetailsFiles { get; }
        IList<ExpressionFile> ExpressionFiles { get; }
		JsonSerializerType JsonSerializerType { get; }

        public Task<BuildChannel> AddBuildChannelAsync(string name, ClassificationType classificationType, string repositoryUrl, string branchName, CancellationToken cancellationToken = default);
        public Task RemoveBuildChannelAsync(string name, CancellationToken cancellationToken = default);
        public Task<BuildChannel> FindBuildChannelByNameAsync(string channelName, CancellationToken cancellationToken = default);
        public Task<bool> AddOrUpdateBuildChannelAsync(BuildInfo buildInfo, string repositoryPath, CancellationToken cancellationToken = default);
        Task<Subscription> FindSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default);
	}
}
