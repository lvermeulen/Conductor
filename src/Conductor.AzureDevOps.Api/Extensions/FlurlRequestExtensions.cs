using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Conductor.AzureDevOps.Api.Extensions
{
	public static class FlurlRequestExtensions
	{
		internal static IList<JToken> GetJsonTokens(this string json) => JObject.Parse(json);

		internal static JProperty GetJsonPropertyByIndex(this IList<JToken> tokens, int index) => (JProperty)tokens[index];

		internal static JProperty GetJsonPropertyByName(this IList<JToken> tokens, string name) => tokens
			.OfType<JProperty>()
			.FirstOrDefault(x => x.Name == name);

		private static async Task<IList<JToken>> GetJsonTokensAsync(this IFlurlRequest request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default)
		{
			var responseMessage = await request.SendAsync(HttpMethod.Get, null, cancellationToken, completionOption).ConfigureAwait(false);
			if (responseMessage == null)
			{
				return default;
			}

			var responseString = await responseMessage.ResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			return responseString.GetJsonTokens();
		}

		private static async Task<JProperty> GetJsonNodeAsync(this IFlurlRequest request, int index, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default)
		{
			var tokens = await request.GetJsonTokensAsync(completionOption, cancellationToken).ConfigureAwait(false);
			return tokens.GetJsonPropertyByIndex(index);
		}

		private static async Task<JProperty> GetJsonNodeAsync(this IFlurlRequest request, string nodeName, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default)
		{
			var tokens = await request.GetJsonTokensAsync(completionOption, cancellationToken).ConfigureAwait(false);
			return tokens.GetJsonPropertyByName(nodeName);
		}

		public static async Task<T> GetJsonFirstNodeAsync<T>(this IFlurlRequest request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default)
		{
			var jproperty = await request.GetJsonNodeAsync(0, completionOption, cancellationToken).ConfigureAwait(false);
			return jproperty.Value.ToObject<T>();
		}

		public static async Task<T> GetJsonFirstNodeAsync<T, TJsonConverter>(this IFlurlRequest request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default)
			where TJsonConverter : JsonConverter, new()
		{
			var serializer = new JsonSerializer
			{
				Converters = { new TJsonConverter() }
			};

			var jproperty = await request.GetJsonNodeAsync(0, completionOption, cancellationToken).ConfigureAwait(false);
			return jproperty.Value.ToObject<T>(serializer);
		}

		public static async Task<T> GetJsonNamedNodeAsync<T>(this IFlurlRequest request, string nodeName, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default)
		{
			var jproperty = await request.GetJsonNodeAsync(nodeName, completionOption, cancellationToken).ConfigureAwait(false);
			return jproperty.Value.ToObject<T>();
		}
	}
}
