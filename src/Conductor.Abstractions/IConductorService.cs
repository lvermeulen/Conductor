using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Conductor.Abstractions
{
	public interface IConductorService
	{
		public ConcurrentDictionary<string, BuildChannel> Channels { get; }
        public IList<ExpressionDetailFile> ExpressionDetailFiles { get; }
        public IList<ExpressionFile> ExpressionFiles { get; }

        public Task<BuildChannel> AddChannelAsync(string name, ClassificationType classificationType, string repositoryUrl, string branchName);
        public Task RemoveChannelAsync(string name);
        public Task<BuildChannel> FindChannelByNameAsync(string channelName);
        public Task<bool> AddOrUpdateBuildChannelAsync(BuildInfo buildInfo, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Dependency>> DownloadAssetsAsync(string url, CancellationToken cancellationToken = default);
	}
}
