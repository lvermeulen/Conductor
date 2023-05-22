using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Conductor.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Conductor.Core
{
	public class SubscriptionsExecutor
	{
		private static readonly GitOperations.GitOperations s_gitOps = new GitOperations.GitOperations();

		private readonly IConductorService _conductor;

		public SubscriptionsExecutor(IConductorService conductor)
		{
			_conductor = conductor;
		}

		private bool IsFileUrl(string url)
		{
			var uri = new Uri(url);
			return uri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase);
		}

		private async Task<DependencyMetadata> GetXmlMetadataAsync(string url, CancellationToken cancellationToken)
		{
			async Task<Stream> GetFileStreamAsync()
			{
				var fileName = new Uri(url).LocalPath;
				var fileContents = await File.ReadAllTextAsync(fileName, cancellationToken);
				var stm = new MemoryStream(Encoding.UTF8.GetBytes(fileContents));
				return stm;
			}

			async Task<Stream> GetHttpStreamAsync()
			{
				using var client = new HttpClient();
				using var result = await client.GetAsync(url, cancellationToken);
				if (!result.IsSuccessStatusCode)
				{
					return Stream.Null;
				}

				var stm = await result.Content.ReadAsStreamAsync(cancellationToken);
				return stm;
			}			

			var stm = IsFileUrl(url)
				? await GetFileStreamAsync()
				: await GetHttpStreamAsync();

			var doc = await XDocument.LoadAsync(stm, LoadOptions.None, cancellationToken);
			var metadata = new Channels.DependencyDetailsReaders.Xml.VersionDetailsXmlReader(doc).ReadMetadata();
			return metadata;
		}

		private async Task<DependencyMetadata> GetNewtonsoftJsonMetadataAsync(string url, CancellationToken cancellationToken)
		{
			async Task<StringReader> GetFileReaderAsync()
			{
				var fileName = new Uri(url).LocalPath;
				var fileContents = await File.ReadAllTextAsync(fileName, cancellationToken);
				var stringReader = new StringReader(fileContents);
				return stringReader;
			}

			async Task<StringReader> GetHttpReaderAsync()
			{
				using var client = new HttpClient();
				using var result = await client.GetAsync(url, cancellationToken);
				if (!result.IsSuccessStatusCode)
				{
					return default;
				}

				var s = await result.Content.ReadAsStringAsync(cancellationToken);
				var stringReader = new StringReader(s);
				return stringReader;
			}

			using var stringReader = IsFileUrl(url)
				? await GetFileReaderAsync()
				: await GetHttpReaderAsync();

            await using var jsonReader = new JsonTextReader(stringReader);
			var doc = await JObject.LoadAsync(jsonReader, null, cancellationToken);
			var metadata = new Channels.DependencyDetailsReaders.NewtonsoftJson.VersionDetailsJsonReader(doc).ReadMetadata();
			return metadata;
		}

		private async Task<DependencyMetadata> GetSystemTextJsonMetadataAsync(string url, CancellationToken cancellationToken)
		{
			async Task<Stream> GetFileStreamAsync()
			{
				var fileName = new Uri(url).LocalPath;
				var fileContents = await File.ReadAllTextAsync(fileName, cancellationToken);
				var stm = new MemoryStream(Encoding.UTF8.GetBytes(fileContents));
				return stm;
			}

			async Task<Stream> GetHttpStreamAsync()
			{
				using var client = new HttpClient();
				using var result = await client.GetAsync(url, cancellationToken);
				if (!result.IsSuccessStatusCode)
				{
					return Stream.Null;
				}

				var stm = await result.Content.ReadAsStreamAsync(cancellationToken);
				return stm;
			}

			await using var stm = IsFileUrl(url)
				? await GetFileStreamAsync()
				: await GetHttpStreamAsync();

			var doc = await JsonDocument.ParseAsync(stm, cancellationToken: cancellationToken);
			var metadata = new Channels.DependencyDetailsReaders.SystemTextJson.VersionDetailsJsonReader(doc).ReadMetadata();
			return metadata;
		}

		private Task<DependencyMetadata> GetMetadataAsync(string url, DependencyFileType dependencyFileType, CancellationToken cancellationToken)
		{
			if (dependencyFileType == DependencyFileType.Xml)
			{
				return GetXmlMetadataAsync(url, cancellationToken);
			}

			return _conductor.JsonSerializerType == JsonSerializerType.NewtonsoftJson
				? GetNewtonsoftJsonMetadataAsync(url, cancellationToken)
				: GetSystemTextJsonMetadataAsync(url, cancellationToken);
		}

		public async Task<IList<Dependency>> DownloadAssetsAsync(string url, CancellationToken cancellationToken = default)
		{
			var results = new List<Dependency>();

			foreach (var dependencyDetailsFile in _conductor.DependencyDetailsFiles)
			{
				var actualUrl = string.IsNullOrWhiteSpace(dependencyDetailsFile.SubPath)
					? $"{url}/"
					: $"{url}/{dependencyDetailsFile.SubPath}/";
				var dependencyDetailsUrl = $"{actualUrl}{dependencyDetailsFile.FileName}";

				var metadata = await GetMetadataAsync(dependencyDetailsUrl, dependencyDetailsFile.DependencyFileType, cancellationToken);
				foreach (var dependency in metadata.Dependencies)
				{
					var name = dependencyDetailsFile.FileName;
					var version = dependency.Version;
					var sha = dependency.Sha;
					var pinned = dependency.Pinned == true;
					var expression = dependency.Expression;

					results.Add(new Dependency(name, version, sha, pinned, expression)
					{
						DependencyType = dependency.DependencyType,
						Expression = expression
					});
				}
			}

			return results;
		}

		private async Task<Build> RenderBuildInfoAsync(BuildInfo buildInfo)
		{
			var channel = await _conductor.FindBuildChannelByNameAsync(buildInfo.ChannelName);
			var assets = await DownloadAssetsAsync(buildInfo.ArtifactsUrl);

			return new Build(channel, assets);
		}

		private async Task<Repository> CheckOutSourcesAsync(string repositoryPath, string subscriptionTargetRepositoryUrl, string subscriptionTargetBranchName, CancellationToken cancellationToken = default)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return default;
			}

			await s_gitOps.CloneRepositoryAsync(subscriptionTargetRepositoryUrl, repositoryPath, cancellationToken);
			await s_gitOps.CheckoutRepositoryBranchAsync(repositoryPath, subscriptionTargetBranchName, cancellationToken);

			return new Repository(repositoryPath);
		}

		private async Task TagTheseUsersOnDependencyFlowPullRequestAsync(CancellationToken cancellationToken = default)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}

			// TODO: implement
			// github message with user names @'ed
		}

		public async Task ExecuteSubscriptionAsync(BuildInfo newBuildInfo, Subscription subscription, string repositoryPath, CancellationToken cancellationToken = default)
		{
			// determine whether the build applies to the subscription
			if (subscription.SourceRepositoryUrl != newBuildInfo.SourceRepositoryUrl || subscription.ChannelName != newBuildInfo.ChannelName)
			{
				return;
			}

			// determine whether trigger should be run, e.g. this might return false if we've already run once today and the subscription only runs once a day
			if (subscription.IsTriggered(newBuildInfo))
			{
				return;
			}

			var (_, assets) = await RenderBuildInfoAsync(newBuildInfo);

			// check out target repo and branch (git clone targetRepo; git checkout targetBranch)
			var repo = await CheckOutSourcesAsync(repositoryPath, subscription.TargetRepositoryUrl, subscription.TargetBranchName, cancellationToken);

			// check out a new branch in which to make a commit (git checkout -b update-dependencies)
			await repo.CheckOutBranchForChangesAsync("update-dependencies", cancellationToken);

			// map assets existing in target
			foreach (var dependency in repo.Dependencies)
			{
				if (assets.Contains(dependency)
					&& (!subscription.Assets.Any() || subscription.Assets.Contains(dependency)))
				{
					await repo.UpdateAssetsAsync(assets, cancellationToken);
				}
			}

            // check quality of new repo content (do a build, etc.)
			if (subscription.IsDesiredQuality(repo))
			{
				await repo.MergeChangesAsync(subscription.TargetBranchName, cancellationToken);
			}
			else if (subscription.HasFailureNotificationTags && subscription.IsNotBatched)
			{
				await TagTheseUsersOnDependencyFlowPullRequestAsync(cancellationToken);
			}
		}
	}
}
