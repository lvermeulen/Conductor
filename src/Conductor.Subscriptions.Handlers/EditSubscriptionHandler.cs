using System;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Core;
using MediatR;

namespace Conductor.Subscriptions.Handlers
{
	public record EditSubscriptionRequest(Guid Id, Subscription Subscription) : IRequest<EditSubscriptionResponse>;
	public record EditSubscriptionResponse(Subscription Subscription);

	public class EditSubscriptionHandler : IRequestHandler<EditSubscriptionRequest, EditSubscriptionResponse>
	{
		private readonly ConductorService _conductor;

		public EditSubscriptionHandler(ConductorService conductor)
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
