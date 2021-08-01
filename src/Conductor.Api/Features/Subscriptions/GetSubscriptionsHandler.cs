using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using MediatR;

namespace Conductor.Api.Features.Subscriptions
{
    public record GetSubscriptionsRequest : IRequest<GetSubscriptionsResponse>;
    public record GetSubscriptionsResponse(IEnumerable<Subscription> Subscriptions);

    public class GetSubscriptionsHandler : IRequestHandler<GetSubscriptionsRequest, GetSubscriptionsResponse>
    {
        private readonly IConductorService _conductor;

        public GetSubscriptionsHandler(IConductorService conductor)
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
