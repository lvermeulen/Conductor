using System.Threading;
using System.Threading.Tasks;
using Conductor.RunProcess;
using LibGit2Sharp;

// ReSharper disable AccessToDisposedClosure
// ReSharper disable PossibleMultipleEnumeration

namespace Conductor.GitOperations
{
	public class GitOperations
	{
		//private static readonly IDictionary<ChangeKind, CliChangeKinds> s_changeKindsByChangeKind = new Dictionary<ChangeKind, CliChangeKinds>
		//{
		//	[ChangeKind.Unmodified] = CliChangeKinds.Unmodified,
		//	[ChangeKind.Added] = CliChangeKinds.Added,
		//	[ChangeKind.Deleted] = CliChangeKinds.Deleted,
		//	[ChangeKind.Modified] = CliChangeKinds.Modified,
		//	[ChangeKind.Renamed] = CliChangeKinds.Renamed,
		//	[ChangeKind.Copied] = CliChangeKinds.Copied,
		//	[ChangeKind.Ignored] = CliChangeKinds.Ignored,
		//	[ChangeKind.Untracked] = CliChangeKinds.Untracked,
		//	[ChangeKind.TypeChanged] = CliChangeKinds.TypeChanged,
		//	[ChangeKind.Unreadable] = CliChangeKinds.Unreadable,
		//	[ChangeKind.Conflicted] = CliChangeKinds.Conflicted
		//};

		//private const string origin = "origin";

		//public Task<string> CreateBareRepositoryAsync(string repositoryPath) => Task.FromResult(Repository.Init(repositoryPath, isBare: true));

		//public Task<string> CreateNonBareRepositoryAsync(string repositoryPath) => Task.FromResult(Repository.Init(repositoryPath));

		//public Task<bool> HasRemoteAsync(string repositoryPath, string remoteName)
		//{
		//	using var repo = new Repository(repositoryPath);
		//	var remote = repo.Network.Remotes.FirstOrDefault(x => x.Name == remoteName);
		//	bool result = remote is not null;

		//	return Task.FromResult(result);
		//}

		//public Task<string> CreateRemoteAsync(string repositoryPath, string remoteName)
		//{
		//	using var repo = new Repository(repositoryPath);
		//	var remote = repo.Network.Remotes.Add(remoteName, repositoryPath);

		//	repo.Branches.Update(repo.Head,
		//		b => b.Remote = remote.Name,
		//		b => b.UpstreamBranch = repo.Head.CanonicalName);

		//	repo.Network.Push(remote, Enumerable.Empty<string>());

		//	return Task.FromResult(remoteName);
		//}

		//public Task<bool> HasChangesAsync(string repositoryPath)
		//{
		//	using var repo = new Repository(repositoryPath);
		//	var options = new StatusOptions
		//	{
		//		IncludeUnaltered = false
		//	};

		//	var status = repo.RetrieveStatus(options);
		//	bool result = status.IsDirty;

		//	return Task.FromResult(result);
		//}

		//public Task<string> GetRepositoryCurrentBranchAsync(string repositoryPath)
		//{
		//	using var repo = new Repository(repositoryPath);
		//	string result = repo.Head.FriendlyName;

		//	return Task.FromResult(result);
		//}

		//public async Task<bool> RemoteBranchExistsAsync(string repositoryPath, string branchName)
		//{
		//	static IEnumerable<string> Parse(string s) => s
		//		.Split(new[] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
		//		.Select(x => x.Trim().Replace("origin/", ""));

		//	var processRunner = new ProcessRunner();
		//	var runResult = await processRunner.RunProcessAsync("git", "branch -r", repositoryPath);
		//	if (runResult.ExitCode == 0)
		//	{
		//		var branches = Parse(runResult.Output);
		//		return branches.Contains(branchName, StringComparer.InvariantCultureIgnoreCase);
		//	}

		//	return false;
		//}

		//public Task<bool> BranchExistsAsync(string repositoryPath, string branchName)
		//{
		//	using var repo = new Repository(repositoryPath);
		//	var branches = repo.Branches.ToList();
		//	bool result = branches.Any(x => x.FriendlyName.Equals(branchName));

		//	return Task.FromResult(result);
		//}

		//public async Task<string> CheckoutBranchAsync(string repositoryPath, string branchName)
		//{
		//	using var repo = new Repository(repositoryPath);
		//	branchName = branchName.ToKebabCase();
		//	var branch = await BranchExistsAsync(repositoryPath, branchName)
		//		? repo.Branches[branchName]
		//		: repo.CreateBranch(branchName);
		//	Commands.Checkout(repo, branch);

		//	return branchName;
		//}

		//public async Task<bool> CreateRemoteBranchAsync(string repositoryPath, string branchName)
		//{
		//	var processRunner = new ProcessRunner();
		//	var runResult = await processRunner.RunProcessAsync("git", $"push -u origin {branchName}", repositoryPath);

		//	return runResult.ExitCode == 0;
		//}

		//public async Task<string> CreateBranchAsync(string repositoryPath, string branchName, bool checkout, bool createRemoteBranch)
		//{
		//	using var repo = new Repository(repositoryPath);
		//	branchName = branchName.ToKebabCase();
		//	var branch = await BranchExistsAsync(repositoryPath, branchName)
		//		? repo.Branches[branchName]
		//		: repo.CreateBranch(branchName);

		//	if (checkout)
		//	{
		//		Commands.Checkout(repo, branch);
		//	}

		//	if (createRemoteBranch)
		//	{
		//		if (!await HasRemoteAsync(repositoryPath, origin))
		//		{
		//			await CreateRemoteAsync(repositoryPath, origin);
		//		}

		//		if (!await RemoteBranchExistsAsync(repositoryPath, branchName))
		//		{
		//			await CreateRemoteBranchAsync(repositoryPath, branchName);
		//		}
		//	}

		//	return branch.FriendlyName;
		//}

		//public async Task<bool> DeleteRemoteBranchAsync(string repositoryPath, string branchName)
		//{
		//	var processRunner = new ProcessRunner();
		//	var runResult = await processRunner.RunProcessAsync("git", $"push origin --delete {branchName}", repositoryPath);

		//	return runResult.ExitCode == 0;
		//}

		//public async Task<bool> DeleteBranchAsync(string repositoryPath, string branchName)
		//{
		//	var processRunner = new ProcessRunner();
		//	var runResult = await processRunner.RunProcessAsync("git", $"branch --delete --force {branchName}", repositoryPath);

		//	return runResult.ExitCode == 0;
		//}

		//private string GetRelativeFileName(string fileName, string path) => Path.GetFullPath(fileName)[path.Length..].Trim('\\');

		//public Task AddFilesAsync(string repositoryPath, params string[] fileNames)
		//{
		//	using var repo = new Repository(repositoryPath);
		//	foreach (string fileName in fileNames)
		//	{
		//		string relativeFileName = GetRelativeFileName(fileName, repositoryPath);
		//		repo.Index.Add(relativeFileName);
		//	}
		//	repo.Index.Write();

		//	return Task.CompletedTask;
		//}

		//public Task<object> CommitAsync(string repositoryPath, string authorName, string authorEmail, string commitMessage)
		//{
		//	using var repo = new Repository(repositoryPath);
		//	var author = new Signature(authorName, authorEmail, DateTime.Now);
		//	var commit = repo.Commit(commitMessage, author, author);

		//	return Task.FromResult((object)commit);
		//}

		//public Task EmptyCommitAsync(string repositoryPath, string authorName, string authorEmail)
		//{
		//	using var repo = new Repository(repositoryPath);
		//	var author = new Signature(authorName, authorEmail, DateTime.Now);
		//	repo.Commit(string.Empty, author, author, new CommitOptions { AllowEmptyCommit = true });

		//	return Task.CompletedTask;
		//}

		////public Task<IEnumerable<CliChange>> GetCommitChangesAsync(string repositoryPath)
		////{
		////	using var repo = new Repository(repositoryPath);
		////	var commitTree = repo.Head.Tip.Tree;
		////	var parentCommitTree = repo.Head.Tip.Parents.First().Tree;
		////	var results = repo.Diff.Compare<TreeChanges>(parentCommitTree, commitTree)
		////		.Select(change => new CliChange(change.Path, s_changeKindsByChangeKind[change.Status]));

		////	return Task.FromResult(results);
		////}

		////public Task<IList<CliCommit>> GetRepositoryCommitsAsync(string repositoryPath)
		////{
		////	using var repo = new Repository(repositoryPath);
		////	var branch = repo.Head;
		////	IList<CliCommit> commits = branch.Commits
		////		.Select(x => new CliCommit(x.Message, x.Author.Name, x.Author.Email, x.Committer.Name, x.Committer.Email, string.Join(Environment.NewLine, x.Notes.Select(note => note.Message))))
		////		.ToList();
		////	return Task.FromResult(commits);
		////}

		////public async Task<RunResult> PushAsync(string repositoryPath, string branchName, NetworkCredential credentials)
		////{
		////	using var repo = new Repository(repositoryPath);
		////	var processRunner = new ProcessRunner();
		////	var runResult = await processRunner.RunProcessAsync("git", $"push {origin} {branchName}", repositoryPath);

		////	return runResult;
		////}

		//public async Task<string> InitializeRepositoryAsync(string repositoryPath)
		//{
		//	await CreateNonBareRepositoryAsync(repositoryPath);
		//	//await s_gitOps.EmptyCommitAsync(repositoryPath, Environment.UserName, "1@2.com");

		//	return repositoryPath;
		//}

		public Task<bool> CloneRepositoryAsync(string repositoryUrl, string repositoryPath, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromResult(false);
			}

			Repository.Clone(repositoryUrl, repositoryPath);
			return Task.FromResult(true);
		}

		public Task<bool> CheckoutRepositoryBranchAsync(string repositoryPath, string branchName, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromResult(false);
			}

			using var repo = new Repository(repositoryPath);
			var branch = repo.Branches[branchName];
			if (branch == null)
			{
				// repository return null object when branch not exists
				return Task.FromResult(false);
			}

			_ = Commands.Checkout(repo, branch);
			return Task.FromResult(true);
		}

		public async Task<bool> MergeChangesAsync(string repositoryPath, string targetBranchName, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return false;
			}

			var processRunner = new ProcessRunner();
			var runResult = await processRunner.RunProcessAsync("git", $"merge --no-ff --no-squash {targetBranchName}", repositoryPath);
			if (runResult.ExitCode == 0)
			{
				return true;
			}

			return false;
		}
	}
}
