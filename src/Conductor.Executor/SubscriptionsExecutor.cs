//using System.Threading;
//using System.Threading.Tasks;

namespace Conductor.Executor
{
	public static class SubscriptionsExecutor
	{
		//public static Task ExecuteSubscriptionAsync(Build newBuild, Subscription subscription, CancellationToken cancellationToken = default)
		//{
		//	// Determine whether the build applies to the subscription
		//	if (subscription.SourceRepositoryUrl != newBuild.sourceRepo || subscription.ChannelName != newBuild.channel)
		//	{
		//		return;
		//	}

		//	// Determine whether trigger should be run
		//	// For example, this might return false if we've already run once today and the subscription only runs once a day.
		//	if (subscription.IsTriggered(newBuild))
		//	{
		//		return;
		//	}

		//	// Check out the target repo and branch.
		//	// git clone targetRepo; git checkout targetBranch
		//	var repo = CheckOutSources(subscription.TargetRepositoryUrl, subscription.TargetBranchName);

		//	// Check out a new branch in which to make a commit
		//	// git checkout -b update-dependencies
		//	repo.checkOutBranchForChanges();

		//	// Map assets existing in target's 
		//	foreach (var dependency in repo.Dependencies) 
		//	{
		//		if (newBuild.assets.contains(dependency))
		//		{
		//			if (subscription.Assets.count == 0 || subscription.Assets.contains(dependency))
		//			{
		//				repo.updateAsset(newBuild.assets);
		//			}
		//		}
		//	}
		//	// Check quality of new repo content (do a build, etc.)
		//	if (subscription.IsDesiredQuality(repo))
		//	{
		//		MergeChanges();
		//	}
		//	else if (subscription.HasFailureNotificationTags && subscription.IsNotBatched)
		//	{
		//		TagTheseUsersOnDependencyFlowPullRequest();
		//	}

		//	return Task.CompletedTask;
		//}
	}
}
