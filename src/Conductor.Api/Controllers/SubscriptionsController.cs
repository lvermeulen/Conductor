using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Conductor.Subscriptions;
using Conductor.Subscriptions.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Conductor.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SubscriptionsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<SubscriptionsController> _logger;

		public SubscriptionsController(IMediator mediator, ILogger<SubscriptionsController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Subscription>), (int)HttpStatusCode.OK)]
		public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync()
		{
			_logger.LogTrace(nameof(AddSubscriptionAsync));

			var result = await _mediator.Send(new GetSubscriptionsRequest());
			return result.Subscriptions;
		}

		[HttpGet]
		[ProducesResponseType(typeof(Subscription), (int)HttpStatusCode.OK)]
		[Route("{id:guid}")]
		public async Task<Subscription> GetSubscriptionAsync([FromRoute] Guid id)
		{
			_logger.LogTrace(nameof(AddSubscriptionAsync));

			var result = await _mediator.Send(new GetSubscriptionRequest(id));
			return result.Subscription;
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<Subscription> AddSubscriptionAsync([FromQuery] string channelName, [FromQuery] string sourceRepositoryUrl, [FromQuery] string targetRepositoryUrl, [FromQuery] string targetBranchName,
			[FromQuery] UpdateFrequency updateFrequency, [FromQuery] IEnumerable<string> policies)
		{
			_logger.LogTrace(nameof(AddSubscriptionAsync));

			var result = await _mediator.Send(new AddSubscriptionRequest(channelName, sourceRepositoryUrl, targetRepositoryUrl, targetBranchName, updateFrequency, policies));
			return result.Subscription;
		}

		[HttpPut]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[Route("{id:guid}")]
		public async Task<Subscription> EditSubscriptionAsync([FromRoute] Guid id, [FromQuery] string channelName, [FromQuery] string sourceRepositoryUrl, [FromQuery] string targetRepositoryUrl,
			[FromQuery] string targetBranchName, [FromQuery] UpdateFrequency updateFrequency, [FromQuery] IEnumerable<string> policies)
		{
			_logger.LogTrace(nameof(AddSubscriptionAsync));

			var subscription = new Subscription(channelName, sourceRepositoryUrl, targetRepositoryUrl, targetBranchName, updateFrequency, policies);
			var result = await _mediator.Send(new EditSubscriptionRequest(id, subscription));
			return result.Subscription;
		}

		[HttpDelete]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[Route("{id:guid}")]
		public async Task RemoveSubscriptionAsync([FromRoute] Guid id)
		{
			_logger.LogTrace(nameof(AddSubscriptionAsync));

			_ = await _mediator.Send(new RemoveSubscriptionRequest(id));
		}
	}
}
