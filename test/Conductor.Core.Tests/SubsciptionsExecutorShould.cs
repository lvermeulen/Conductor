using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Channels.DependencyDetailsReaders.SystemTextJson.Extensions;
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

		[Fact]
		public async Task ExecuteSubscriptionAsync()
        {
            bool success = false;

			var fileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			var dir = Directory.CreateDirectory(fileName);
			var repositoryPath = dir.FullName;

            var dependencies = new List<Dependency>
            {
                new Dependency("Newtonsoft.Json", "11.0.1", "some-sha")
            };
            var metadata = new DependencyMetadata(dependencies);
            await metadata.WriteSystemTextJson("VersionDetails.json");

			using (new AutoCleanupFolder(repositoryPath))
			{
				const string sourceRepositoryUrl = "https://github.com/lvermeulen/BuildMaster.Net.git";
				var channel = await _conductor.AddBuildChannelAsync(sourceRepositoryUrl, ClassificationType.Product, sourceRepositoryUrl, "master", CancellationToken.None);
				var artifactsUrl = new Uri(Environment.CurrentDirectory, UriKind.Absolute).LocalPath;
				const string targetRepositoryUrl = "https://github.com/lvermeulen/ProGet.Net.git";
				const string targetBranchName = "master";
				var subscription = await channel.AddSubscriptionAsync(sourceRepositoryUrl, targetRepositoryUrl, targetBranchName, UpdateFrequency.Daily, Enumerable.Empty<string>(), CancellationToken.None);

				var newBuild = new BuildInfo(sourceRepositoryUrl, channel.Name, artifactsUrl);

				await _subsciptionsExecutor.ExecuteSubscriptionAsync(newBuild, subscription, repositoryPath, () => success = true, CancellationToken.None);
			}

			Assert.True(success);
			Assert.False(Directory.Exists(repositoryPath));
		}
	}
}
