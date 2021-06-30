using System;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Core;
using MediatR;

namespace Conductor.Subscriptions.Handlers
{
	public record GetSubscriptionRequest(Guid Id) : IRequest<GetSubscriptionResponse>;
	public record GetSubscriptionResponse(Subscription Subscription);

	public class GetSubscriptionHandler : IRequestHandler<GetSubscriptionRequest, GetSubscriptionResponse>
	{
		private readonly ConductorService _conductor;

		public GetSubscriptionHandler(ConductorService conductor)
		{
			_conductor = conductor;
		}

		public Task<GetSubscriptionResponse> Handle(GetSubscriptionRequest request, CancellationToken cancellationToken)
		{
			foreach (var channel in _conductor.Channels)
			{
				foreach (var subscription in channel.Value.Subscriptions)
				{
					if (subscription.Id == request.Id)
					{
						var result = new GetSubscriptionResponse(subscription);
						return Task.FromResult(result);
					}
				}
			}

			return Task.FromResult(new GetSubscriptionResponse(default));
		}
	}
}
