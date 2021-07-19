using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;

namespace Conductor.Executor
{
    public class SubscriptionsExecutor
    {
	    private readonly IConductorService _conductor;

	    public SubscriptionsExecutor(IConductorService conductor)
	    {
		    _conductor = conductor;
	    }

	    private async Task<Build> RenderBuildInfoAsync(BuildInfo buildInfo)
	    {
		    var channel = await _conductor.FindChannelByNameAsync(buildInfo.ChannelName);
		    var assets = await _conductor.DownloadAssetsAsync(buildInfo.ArtifactsUrl);

		    return new Build
		    {
                SourceRepository = buildInfo.SourceRepository,
                ChannelName = buildInfo.ChannelName,
                ArtifactsUrl = buildInfo.ArtifactsUrl,
                Channel = channel,
                Assets = assets
		    };
	    }

        public async Task ExecuteSubscriptionAsync(BuildInfo newBuildInfo, Subscription subscription, CancellationToken cancellationToken = default)
        {
            // determine whether the build applies to the subscription
            if (subscription.SourceRepositoryUrl != newBuildInfo.SourceRepository || subscription.ChannelName != newBuildInfo.ChannelName)
            {
                return;
            }

            // determine whether trigger should be run, e.g. this might return false if we've already run once today and the subscription only runs once a day
            if (subscription.IsTriggered(newBuildInfo))
            {
                return;
            }

            var newBuild = await RenderBuildInfoAsync(newBuildInfo);

            // check out target repo and branch (git clone targetRepo; git checkout targetBranch)
            var repo = await CheckOutSources(subscription.TargetRepositoryUrl, subscription.TargetBranchName, cancellationToken);

            // check out a new branch in which to make a commit (git checkout -b update-dependencies)
            await repo.CheckOutBranchForChanges();

            // map assets existing in target
            foreach (var dependency in repo.Dependencies)
            {
                if (newBuild.Assets.Contains(dependency)
                    && (!subscription.Assets.Any() || subscription.Assets.Contains(dependency)))
                {
                    await repo.UpdateAssets(newBuild.Assets);
                }
            }
            // check quality of new repo content (do a build, etc.)
            if (subscription.IsDesiredQuality(repo))
            {
                await MergeChanges(cancellationToken);
            }
            else if (subscription.HasFailureNotificationTags && subscription.IsNotBatched)
            {
                await TagTheseUsersOnDependencyFlowPullRequest(cancellationToken);
            }
        }

        private static Task<Repository> CheckOutSources(string subscriptionTargetRepositoryUrl, string subscriptionTargetBranchName, CancellationToken cancellationToken = default)
        {
	        // TODO: implement
	        return Task.FromResult(new Repository());
        }

        private static Task MergeChanges(CancellationToken cancellationToken = default)
        {
	        // TODO: implement
	        return Task.FromResult(true);
        }

        private static Task TagTheseUsersOnDependencyFlowPullRequest(CancellationToken cancellationToken = default)
        {
	        // TODO: implement
	        return Task.FromResult(true);
        }
    }
}
