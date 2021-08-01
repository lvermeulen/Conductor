using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Conductor.AzureDevOps.Api.Extensions;
using Conductor.AzureDevOps.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Conductor.AzureDevOps.Api
{
	public class AzureDevOpsApiWrapper
	{
		private static readonly AzureDevOpsApi s_api = new AzureDevOpsApi();

		public async Task<IEnumerable<AzureDevOpsUser>> ListUserEntitlementsAsync(string organization, NetworkCredential credentials)
		{
			var result = await s_api.ListUserEntitlementsAsync(organization, credentials);
			return result
				.Select(x => new AzureDevOpsUser(new Guid(x.Id), x.User.DisplayName, x.User.MailAddress));
		}

		public async Task<IEnumerable<AzureDevOpsProject>> ListProjectsAsync(string organization, NetworkCredential credentials)
		{
			var result = await s_api.ListProjectsAsync(organization, credentials);
			return result
				.Select(x => new AzureDevOpsProject(x.Id, x.Name));
		}

		public async Task<IEnumerable<AzureDevOpsRepository>> ListRepositoriesAsync(string organization, string projectName, NetworkCredential credentials)
		{
			var result = await s_api.ListRepositoriesAsync(organization, projectName, credentials);
			return result
				.Select(x => new AzureDevOpsRepository(x.Name, x.DefaultBranch, x.RemoteUrl));
		}

		public async Task<string> GetRepositoryDefaultBranchAsync(string organization, string projectName, NetworkCredential credentials, string repositoryName)
		{
			var repos = await ListRepositoriesAsync(organization, projectName, credentials);
			return repos
				.FirstOrDefault(x => x.Name.Equals(repositoryName, StringComparison.InvariantCultureIgnoreCase))
				?.DefaultBranch;
		}

		public object MakePullRequest(string sourceBranchName, IEnumerable<object> reviewerIds, string targetBranchName = "main")
		{
			return new
			{
				sourceRefName = sourceBranchName.MakeRefSpec(),
				targetRefName = targetBranchName.MakeRefSpec(),
				title = sourceBranchName,
				description = sourceBranchName,
				reviewers = reviewerIds
			};
		}

		public string SerializePullRequest(string sourceBranchName, IEnumerable<object> reviewerIds, string targetBranchName = "main")
		{
			return JsonConvert.SerializeObject(MakePullRequest(sourceBranchName, reviewerIds, targetBranchName), new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				NullValueHandling = NullValueHandling.Ignore
			});
		}

		public async Task<IEnumerable<object>> GetReviewerIdsByEmailAsync(string organization, NetworkCredential credentials, params string[] emailAddresses)
		{
			var results = (await s_api.ListUserEntitlementsAsync(organization, credentials))
				.Where(x => emailAddresses.Any(email => x.User.MailAddress.Equals(email, StringComparison.InvariantCultureIgnoreCase)))
				.Select(x => new
				{
					id = $"{x.Id}"
				});

			return results;
		}

		public async Task<GitPullRequest> CreatePullRequestAsync(string organization, string projectName, NetworkCredential credentials, string repositoryName, string branchName,
			IEnumerable<string> reviewerEmailAddresses, bool autoComplete)
		{
			var repository = (await s_api.ListRepositoriesAsync(organization, projectName, credentials))
				.FirstOrDefault(x => x.Name.Equals(repositoryName, StringComparison.InvariantCultureIgnoreCase));
			if (repository is null)
			{
				return default;
			}

			// select reviewers
			var reviewerIds = await GetReviewerIdsByEmailAsync(organization, credentials, reviewerEmailAddresses.ToArray());

			object body = MakePullRequest(branchName, reviewerIds);
			var result = await s_api.CreatePullRequestAsync(organization, projectName, repository.Id, credentials, body);
			if (autoComplete)
			{
				result = await s_api.AutoCompletePullRequestAsync(organization, projectName, repository.Id, credentials, result.PullRequestId, result.CreatedBy);
			}

			return result;
		}

		public async Task<GitPullRequest> AbandonPullRequestAsync(string organization, string projectName, NetworkCredential credentials, string repositoryName, int pullRequestId)
		{
			var repository = (await s_api.ListRepositoriesAsync(organization, projectName, credentials))
				.FirstOrDefault(x => x.Name.Equals(repositoryName, StringComparison.InvariantCultureIgnoreCase));
			if (repository is null)
			{
				return default;
			}

			var result = await s_api.AbandonPullRequestAsync(organization, projectName, repository.Id, credentials, pullRequestId);
			return result;
		}
	}
}
