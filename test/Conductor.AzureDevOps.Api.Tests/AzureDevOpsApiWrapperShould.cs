using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BizAsync.Cli.Abstractions;
using BizAsync.Cli.Tests;
using CloneRepositories;
using GitOpsRepositories;
using GitOpsRepositories.Extensions;
using PullRepositories;
using Xunit;

namespace AzureDevOps.Api.Tests
{
	public class AzureDevOpsApiWrapperShould : BizAsyncCliTestBase
	{
		private static readonly AzureDevOpsApiWrapper s_apiWrapper = new AzureDevOpsApiWrapper();
		private static readonly GitOperations s_gitOps = new GitOperations();
		private static readonly ICloneRepository s_cloneRepository = new CloneRepository();
		private static readonly IPullRepository s_pullRepository = new PullRepository();

		[Fact]
		public async Task ListUserEntitlementsAsync()
		{
			string userName = Environment.UserName;

			var result = await s_apiWrapper.ListUserEntitlementsAsync("engelenconsultants", new NetworkCredential(userName, PersonalAccessToken));
			Assert.NotNull(result);
			Assert.NotEmpty(result);
		}

		[Fact]
		public async Task ListProjectsAsync()
		{
			string userName = Environment.UserName;

			var result = await s_apiWrapper.ListProjectsAsync("engelenconsultants", new NetworkCredential(userName, PersonalAccessToken));
			Assert.NotNull(result);
			Assert.NotEmpty(result);
		}

		[Fact]
		public async Task ListRepositoriesAsync()
		{
			string userName = Environment.UserName;

			var result = await s_apiWrapper.ListRepositoriesAsync("engelenconsultants", "BizAsync", new NetworkCredential(userName, PersonalAccessToken));
			Assert.NotNull(result);
			Assert.NotEmpty(result);
		}

		[Fact]
		public async Task GetReviewerIdsByEmailAsync()
		{
			var result = await s_apiWrapper.GetReviewerIdsByEmailAsync("engelenconsultants", new NetworkCredential(Environment.UserName, PersonalAccessToken), "bjorn.engelen@engelenconsultants.be", "luk.vermeulen@gmail.com");
			Assert.NotNull(result);
		}

		[Fact]
		public async Task CreateAutoCompleteAbandonPullRequestAsync()
		{
			static void Touch(string fileName)
			{
				File.SetLastWriteTimeUtc(fileName, DateTime.UtcNow);
			}

			const string organization = "engelenconsultants";
			const string projectName = "BizAsync";
			const string repoName = "TestRepo";
			
			string repositoryPath = Path.Combine(Environment.CurrentDirectory, "test");
			var credentials = new NetworkCredential(Environment.UserName, PersonalAccessToken);

			using (new AutoCleanupFolder(repositoryPath))
			{
				// clone & pull repo
				var repo = new CliRepository(repoName, "main", "https://engelenconsultants@dev.azure.com/engelenconsultants/BizAsync/_git/TestRepo");
				await s_cloneRepository.CloneRepositoryAsync(repo, repositoryPath, credentials);
				await s_pullRepository.PullRepositoryAsync(repo, repositoryPath, credentials, "1@2.com");

				// create branch
				string branchName = DateTime.UtcNow.ToString("O");
				branchName = branchName.ToKebabCase();
				branchName = await s_gitOps.CreateBranchAsync(repositoryPath, branchName, true, true);

				// touch a file
				Touch(Path.Combine(repositoryPath, "README.md"));

				// create pr
				var result = await s_apiWrapper.CreatePullRequestAsync(organization, projectName, credentials, repoName, branchName, new []{ "bjorn.engelen@engelenconsultants.be", "luk.vermeulen@gmail.com" }, true);
				Assert.NotNull(result);

				// abandon pr
				result = await s_apiWrapper.AbandonPullRequestAsync(organization, projectName, credentials, repoName, result.PullRequestId);
				Assert.NotNull(result);

				// delete source branch
				await s_gitOps.DeleteRemoteBranchAsync(repositoryPath, branchName);
			}
		}
	}
}
