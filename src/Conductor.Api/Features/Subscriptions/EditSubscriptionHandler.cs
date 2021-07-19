using System;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Core;
using MediatR;

namespace Conductor.Api.Features.Subscriptions
{
    public record EditSubscriptionRequest(Guid Id, Subscription Subscription) : IRequest<EditSubscriptionResponse>;
    public record EditSubscriptionResponse(Subscription Subscription);

    public class EditSubscriptionHandler : IRequestHandler<EditSubscriptionRequest, EditSubscriptionResponse>
    {
        private readonly IConductorService _conductor;

        public EditSubscriptionHandler(IConductorService conductor)
        {
            _conductor = conductor;
        }

        public async Task<EditSubscriptionResponse> Handle(EditSubscriptionRequest request, CancellationToken cancellationToken)
        {
            var (guid, subscription) = request;
            var channel = await _conductor.FindChannelByNameAsync(subscription.ChannelName);
            if (channel is null)
            {
                return null;
            }

            subscription = await channel.EditSubscriptionAsync(guid, subscription.SourceRepositoryUrl, subscription.TargetRepositoryUrl, subscription.TargetBranchName, subscription.UpdateFrequency, subscription.Policies);

            return new EditSubscriptionResponse(subscription);
        }
    }
}
