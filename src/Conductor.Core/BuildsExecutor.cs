using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;

namespace Conductor.Core
{
	public class BuildsExecutor
	{
		private static readonly Dictionary<Subscription, IEnumerable<BuildInfo>> s_subscriptionBuildInfos = new Dictionary<Subscription, IEnumerable<BuildInfo>>();

		private readonly IConductorService _conductor;
		private readonly List<BuildInfo> _triggeredBuildInfos = new List<BuildInfo>();

		public BuildsExecutor(IConductorService conductor)
		{
			_conductor = conductor;
		}

		public async Task<BuildInfo> ExecuteBuildInfoAsync(BuildInfo buildInfo, CancellationToken cancellationToken)
		{
			_triggeredBuildInfos.Add(buildInfo);

			// TODO: start build

			return buildInfo;
		}

		public async Task<bool> ExecuteBuildInfosAsync(CancellationToken cancellationToken = default)
		{
			foreach (var subscription in s_subscriptionBuildInfos.Keys)
			{
				foreach (var buildInfo in s_subscriptionBuildInfos[subscription])
				{
					var newBuildInfo = await ExecuteBuildInfoAsync(buildInfo, cancellationToken);
					var channel = await _conductor.FindBuildChannelByNameAsync(subscription.ChannelName, cancellationToken);
					await channel.AddOrUpdateBuildAsync(newBuildInfo, cancellationToken);
				}
			}

			return true;
		}

		public Task<bool> IsBuildTriggered(BuildInfo buildInfo, CancellationToken cancellationToken) => cancellationToken.IsCancellationRequested
			? default
			: Task.FromResult(_triggeredBuildInfos.Contains(buildInfo));
	}
}
