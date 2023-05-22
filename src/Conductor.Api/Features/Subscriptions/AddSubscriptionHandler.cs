using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using MediatR;

namespace Conductor.Api.Features.Subscriptions
{
    public record AddSubscriptionRequest(string ChannelName, string SourceRepositoryUrl, string TargetRepositoryUrl, string TargetBranchName, UpdateFrequency UpdateFrequency, IEnumerable<string> Policies) : IRequest<AddSubscriptionResponse>;

    public record AddSubscriptionResponse(Subscription Subscription);

    public class AddSubscriptionHandler : IRequestHandler<AddSubscriptionRequest, AddSubscriptionResponse>
    {
        private readonly IConductorService _conductor;

        public AddSubscriptionHandler(IConductorService conductor)
        {
            _conductor = conductor;
        }

        public async Task<AddSubscriptionResponse> Handle(AddSubscriptionRequest request, CancellationToken cancellationToken)
        {
            var channel = await _conductor.FindBuildChannelByNameAsync(request.ChannelName, cancellationToken);
            if (channel is null)
            {
                return default;
            }

            var subscription = await channel.AddSubscriptionAsync(request.SourceRepositoryUrl, request.TargetRepositoryUrl, request.TargetBranchName, request.UpdateFrequency, request.Policies, cancellationToken);
            return new AddSubscriptionResponse(subscription);
        }
    }
}
