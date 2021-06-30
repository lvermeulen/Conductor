using System;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Core;
using MediatR;

namespace Conductor.Subscriptions.Handlers
{
	public record RemoveSubscriptionRequest(Guid Id) : IRequest<RemoveSubscriptionResponse>;
	public record RemoveSubscriptionResponse(Subscription Subscription);

	public class RemoveSubscriptionHandler : IRequestHandler<RemoveSubscriptionRequest, RemoveSubscriptionResponse>
	{
		private readonly ConductorService _conductor;

		public RemoveSubscriptionHandler(ConductorService conductor)
		{
			_conductor = conductor;
		}

		public async Task<RemoveSubscriptionResponse> Handle(RemoveSubscriptionRequest request, CancellationToken cancellationToken)
		{
			foreach (var channel in _conductor.Channels.Values)
			{
				foreach (var subscription in channel.Subscriptions)
				{
					if (subscription.Id == request.Id)
					{
						var parentChannel = await _conductor.FindChannelByNameAsync(subscription.ChannelName);
						await parentChannel.RemoveSubscriptionAsync(subscription);
						return new RemoveSubscriptionResponse(subscription);
					}
				}
			}

			return new RemoveSubscriptionResponse(default);
		}
	}
}
