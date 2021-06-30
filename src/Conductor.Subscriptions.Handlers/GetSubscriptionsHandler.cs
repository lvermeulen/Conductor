using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Core;
using MediatR;

namespace Conductor.Subscriptions.Handlers
{
	public record GetSubscriptionsRequest : IRequest<GetSubscriptionsResponse>;
	public record GetSubscriptionsResponse(IEnumerable<Subscription> Subscriptions);

	public class GetSubscriptionsHandler : IRequestHandler<GetSubscriptionsRequest, GetSubscriptionsResponse>
	{
		private readonly ConductorService _conductor;

		public GetSubscriptionsHandler(ConductorService conductor)
		{
			_conductor = conductor;
		}

		public Task<GetSubscriptionsResponse> Handle(GetSubscriptionsRequest request, CancellationToken cancellationToken)
		{
			var result = new List<Subscription>();

			foreach (var channel in _conductor.Channels)
			{
				result.AddRange(channel.Value.Subscriptions);
			}

			return Task.FromResult(new GetSubscriptionsResponse(result));
		}
	}
}
