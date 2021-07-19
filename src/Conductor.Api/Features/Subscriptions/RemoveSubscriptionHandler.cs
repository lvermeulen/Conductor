using System;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Core;
using MediatR;

namespace Conductor.Api.Features.Subscriptions
{
    public record RemoveSubscriptionRequest(Guid Id) : IRequest<RemoveSubscriptionResponse>;
    public record RemoveSubscriptionResponse(Subscription Subscription);

    public class RemoveSubscriptionHandler : IRequestHandler<RemoveSubscriptionRequest, RemoveSubscriptionResponse>
    {
        private readonly IConductorService _conductor;

        public RemoveSubscriptionHandler(IConductorService conductor)
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
