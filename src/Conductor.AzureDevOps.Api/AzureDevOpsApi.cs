using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Conductor.AzureDevOps.Api.Extensions;
using Conductor.AzureDevOps.Api.Models;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Conductor.AzureDevOps.Api
{
	public class AzureDevOpsApi
	{
		private static readonly ISerializer s_serializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			NullValueHandling = NullValueHandling.Ignore
		});

		public async Task<IEnumerable<UserEntitlement>> ListUserEntitlementsAsync(string organization, NetworkCredential credentials)
		{
			var result = await $"https://vsaex.dev.azure.com/{organization}/_apis/userentitlements?api-version=6.0-preview.3"
				.ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
				.WithBasicAuth(credentials.UserName, credentials.Password)
				.WithHeader("Accept", "application/json")
				.GetJsonNamedNodeAsync<IEnumerable<UserEntitlement>>("members")
				.ConfigureAwait(false);

			return result;
		}

		public async Task<IEnumerable<TeamProjectReference>> ListProjectsAsync(string organization, NetworkCredential credentials)
		{
			var result = await $"https://dev.azure.com/{organization}/_apis/projects?api-version=6.0"
				.ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
				.WithBasicAuth(credentials.UserName, credentials.Password)
				.WithHeader("Accept", "application/json")
				.GetJsonNamedNodeAsync<IEnumerable<TeamProjectReference>>("value")
				.ConfigureAwait(false);

			return result;
		}

		public async Task<IEnumerable<GitRepository>> ListRepositoriesAsync(string organization, string projectName, NetworkCredential credentials)
		{
			var result = await $"https://dev.azure.com/{organization}/{projectName}/_apis/git/repositories?api-version=6.0"
				.ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
				.WithBasicAuth(credentials.UserName, credentials.Password)
				.WithHeader("Accept", "application/json")
				.GetJsonFirstNodeAsync<IEnumerable<GitRepository>>()
				.ConfigureAwait(false);

			return result;
		}

		public async Task<GitPullRequest> CreatePullRequestAsync(string organization, string projectName, string repositoryId, NetworkCredential credentials, object body)
		{
			var result = await $"https://dev.azure.com/{organization}/{projectName}/_apis/git/repositories/{repositoryId}/pullrequests?api-version=6.0"
				.ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
				.WithBasicAuth(credentials.UserName, credentials.Password)
				.WithHeader("Accept", "application/json")
				.PostJsonAsync(body)
				.ReceiveJson<GitPullRequest>()
				.ConfigureAwait(false);

			return result;
		}

		private async Task<GitPullRequest> UpdatePullRequestAsync(string organization, string projectName, string repositoryId, NetworkCredential credentials, int pullRequestId, object body)
		{
			var result = await $"https://dev.azure.com/{organization}/{projectName}/_apis/git/repositories/{repositoryId}/pullrequests/{pullRequestId}?api-version=6.0"
				.ConfigureRequest(settings => settings.JsonSerializer = s_serializer)
				.WithBasicAuth(credentials.UserName, credentials.Password)
				.WithHeader("Accept", "application/json")
				.PatchJsonAsync(body)
				.ReceiveJson<GitPullRequest>()
				.ConfigureAwait(false);

			return result;
		}

		public async Task<GitPullRequest> AutoCompletePullRequestAsync(string organization, string projectName, string repositoryId, NetworkCredential credentials, int pullRequestId, IdentityRef autoCompleteSetByIdentityRef)
		{
			var body = new
			{
				autoCompleteSetBy = autoCompleteSetByIdentityRef
			};
			var result = await UpdatePullRequestAsync(organization, projectName, repositoryId, credentials, pullRequestId, body);
			return result;
		}

		public async Task<GitPullRequest> AbandonPullRequestAsync(string organization, string projectName, string repositoryId, NetworkCredential credentials, int pullRequestId)
		{
			var body = new
			{
				status = PullRequestStatus.Abandoned.ToString().ToLowerInvariant()
			};
			var result = await UpdatePullRequestAsync(organization, projectName, repositoryId, credentials, pullRequestId, body);
			return result;
		}

		public async Task<GitPullRequest> DeletePullRequestSourceBranchAsync(string organization, string projectName, string repositoryId, NetworkCredential credentials, int pullRequestId)
		{
			var body = new
			{
				completionOptions = new GitPullRequestCompletionOptions
				{
					DeleteSourceBranch = true
				}
			};
			var result = await UpdatePullRequestAsync(organization, projectName, repositoryId, credentials, pullRequestId, body);
			return result;
		}
	}
}
