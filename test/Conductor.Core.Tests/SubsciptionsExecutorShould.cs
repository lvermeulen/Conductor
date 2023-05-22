using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Tests;
using Xunit;

namespace Conductor.Core.Tests
{
	public class SubsciptionsExecutorShould
	{
		private readonly IConductorService _conductor;
		private readonly SubscriptionsExecutor _subsciptionsExecutor;

		public SubsciptionsExecutorShould()
		{
			_conductor = new ConductorBuilder()
				.WithDependencyDetailsFile("VersionDetails.json", "", DependencyFileType.Json)
				.WithJsonSerializer(JsonSerializerType.SystemTextJson)
				.Build();
			_subsciptionsExecutor = new SubscriptionsExecutor(_conductor);
		}

		[Fact(Skip = "Not ready yet")]
		public async Task ExecuteSubscriptionAsync()
		{
			var fileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			var dir = Directory.CreateDirectory(fileName);
			var repositoryPath = dir.FullName;

			using (new AutoCleanupFolder(repositoryPath))
			{
				const string sourceRepositoryUrl = "https://github.com/lvermeulen/BuildMaster.Net.git";
				var channel = await _conductor.AddBuildChannelAsync(sourceRepositoryUrl, ClassificationType.Product, sourceRepositoryUrl, "master", CancellationToken.None);
				var artifactsUrl = new Uri(Environment.CurrentDirectory, UriKind.Absolute).LocalPath;
				const string targetRepositoryUrl = "https://github.com/lvermeulen/ProGet.Net.git";
				const string targetBranchName = "master";
				var subscription = await channel.AddSubscriptionAsync(sourceRepositoryUrl, targetRepositoryUrl, targetBranchName, UpdateFrequency.Daily, Enumerable.Empty<string>(), CancellationToken.None);

				var newBuild = new BuildInfo(sourceRepositoryUrl, channel.Name, artifactsUrl);

				await _subsciptionsExecutor.ExecuteSubscriptionAsync(newBuild, subscription, repositoryPath, CancellationToken.None);
			}

			Assert.False(Directory.Exists(repositoryPath));
		}
	}
}
