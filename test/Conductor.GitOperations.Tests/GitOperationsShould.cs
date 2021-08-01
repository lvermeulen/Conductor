using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BizAsync.Cli.Abstractions;
using BizAsync.Cli.Tests;
using LibGit2Sharp;
using Xunit;
using Xunit.Abstractions;

namespace GitOpsRepositories.Tests
{
	public class GitOperationsShould : BizAsyncCliTestBase
	{
		private static readonly GitOperations s_gitOps = new GitOperations();

		private readonly ITestOutputHelper _testOutputHelper;

		private static CliParameters GetCliParameters(string personalAccessToken) => new CliParameters("engelenconsultants", "BizAsync", new NetworkCredential(Environment.UserName, personalAccessToken), "1@2.com");

		public GitOperationsShould(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		private async Task<string> InitializeRepositoryAsync(string repositoryPath)
		{
			await s_gitOps.CreateNonBareRepositoryAsync(repositoryPath);
			await s_gitOps.EmptyCommitAsync(repositoryPath, Environment.UserName, "1@2.com");

			return repositoryPath;
		}

		private async Task<string> InitializeBareRepositoryAsync(string repositoryPath)
		{
			await s_gitOps.CreateBareRepositoryAsync(repositoryPath);

			return repositoryPath;
		}

		[Fact]
		public async Task CreateBranchAsync()
		{
			string repositoryPath = await InitializeRepositoryAsync(Path.Combine(Environment.CurrentDirectory, @$"test\{nameof(CreateBranchAsync)}"));
			using (new AutoCleanupFolder(repositoryPath))
			{
				string branchName = await s_gitOps.CreateBranchAsync(repositoryPath, nameof(CreateBranchAsync), checkout: true, createRemoteBranch: false);
				Assert.NotNull(branchName);
			}
		}

		[Fact]
		public async Task CreateRemoteBranchAsync()
		{
			string repositoryPath = await InitializeRepositoryAsync(Path.Combine(Environment.CurrentDirectory, @$"test\{nameof(CreateBranchAsync)}"));
			using (new AutoCleanupFolder(repositoryPath))
			{
				string branchName = await s_gitOps.CreateBranchAsync(repositoryPath, nameof(CreateBranchAsync), checkout: true, createRemoteBranch: true);
				Assert.NotNull(branchName);
			}
		}

		[Fact]
		public async Task AddFilesAsync()
		{
			string repositoryPath = await InitializeRepositoryAsync(Path.Combine(Environment.CurrentDirectory, @$"test\{nameof(AddFilesAsync)}"));
			using (new AutoCleanupFolder(repositoryPath))
			{
				Assert.False(await s_gitOps.HasChangesAsync(repositoryPath));

				string fileName = Path.Combine(repositoryPath, nameof(AddFilesAsync));
				await File.WriteAllTextAsync(fileName, nameof(AddFilesAsync));
				await s_gitOps.AddFilesAsync(repositoryPath, fileName);

				Assert.True(await s_gitOps.HasChangesAsync(repositoryPath));
			}
		}

		[Fact]
		public async Task CommitAsync()
		{
			string repositoryPath = await InitializeRepositoryAsync(Path.Combine(Environment.CurrentDirectory, @$"test\{nameof(CommitAsync)}"));
			using (new AutoCleanupFolder(repositoryPath))
			{
				// add files
				string fileName = Path.Combine(repositoryPath, nameof(AddFilesAsync));
				await File.WriteAllTextAsync(fileName, nameof(AddFilesAsync));
				await s_gitOps.AddFilesAsync(repositoryPath, fileName);

				var cliParameters = GetCliParameters(PersonalAccessToken);
				var commit = await s_gitOps.CommitAsync(repositoryPath, cliParameters.Credentials.UserName, cliParameters.GitEmail, nameof(CommitAsync)) as Commit;
				Assert.NotNull(commit);
			}
		}

		[Fact]
		public async Task GetRepositoryCommitsAsync()
		{
			string repositoryPath = await InitializeRepositoryAsync(Path.Combine(Environment.CurrentDirectory, @$"test\{nameof(GetRepositoryCommitsAsync)}"));
			using (new AutoCleanupFolder(repositoryPath))
			{
				// add files
				string fileName = Path.Combine(repositoryPath, nameof(AddFilesAsync));
				await File.WriteAllTextAsync(fileName, nameof(AddFilesAsync));
				await s_gitOps.AddFilesAsync(repositoryPath, fileName);

				var cliParameters = GetCliParameters(PersonalAccessToken);
				var commit = await s_gitOps.CommitAsync(repositoryPath, cliParameters.Credentials.UserName, cliParameters.GitEmail, nameof(CommitAsync)) as Commit;
				Assert.NotNull(commit);

				var commits = await s_gitOps.GetRepositoryCommitsAsync(repositoryPath);
				var commitList = commits.ToList();
				foreach (var item in commitList)
				{
					_testOutputHelper.WriteLine($"{item.AuthorName} ({item.AuthorEmail}): {item.Message}");
				}
				Assert.NotNull(commitList);
				Assert.NotEmpty(commitList);
			}
		}

		[Fact]
		public async Task GetCommitChangesAsync()
		{
			string repositoryPath = await InitializeRepositoryAsync(Path.Combine(Environment.CurrentDirectory, @$"test\{nameof(GetCommitChangesAsync)}"));
			using (new AutoCleanupFolder(repositoryPath))
			{
				// add files
				string fileName = Path.Combine(repositoryPath, nameof(AddFilesAsync));
				await File.WriteAllTextAsync(fileName, nameof(AddFilesAsync));
				await s_gitOps.AddFilesAsync(repositoryPath, fileName);

				var cliParameters = GetCliParameters(PersonalAccessToken);
				var commit = await s_gitOps.CommitAsync(repositoryPath, cliParameters.Credentials.UserName, cliParameters.GitEmail, nameof(CommitAsync)) as Commit;
				Assert.NotNull(commit);

				var changes = (await s_gitOps.GetCommitChangesAsync(repositoryPath)).ToList();
				foreach (var change in changes)
				{
					_testOutputHelper.WriteLine($"{change.ChangeKind}: {change.Path}");
				}
				Assert.NotNull(changes);
				Assert.NotEmpty(changes);
			}
		}

		[Fact]
		public async Task PushAsync()
		{
			string repositoryPath = await InitializeRepositoryAsync(Path.Combine(Environment.CurrentDirectory, @$"test\{nameof(PushAsync)}"));
			using (new AutoCleanupFolder(repositoryPath))
			{
				// create branch
				await s_gitOps.CreateBranchAsync(repositoryPath, nameof(PushAsync), checkout: true, createRemoteBranch: true);

				// add files
				string fileName = Path.Combine(repositoryPath, nameof(AddFilesAsync));
				await File.WriteAllTextAsync(fileName, nameof(AddFilesAsync));
				await s_gitOps.AddFilesAsync(repositoryPath, fileName);

				var cliParameters = GetCliParameters(PersonalAccessToken);
				var commit = await s_gitOps.CommitAsync(repositoryPath, cliParameters.Credentials.UserName, cliParameters.GitEmail, nameof(CommitAsync)) as Commit;
				Assert.NotNull(commit);

				var runResult = await s_gitOps.PushAsync(repositoryPath, "master", cliParameters.Credentials);
				_testOutputHelper.WriteLine(runResult.Output);
				Assert.NotNull(runResult);
				Assert.Equal(0, runResult.ExitCode);
				Assert.NotNull(runResult.Output);
				Assert.True(runResult.Output.Contains("Everything up-to-date", StringComparison.InvariantCulture));
			}
		}
	}
}
