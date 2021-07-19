using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Conductor.Api.Features.Subscriptions
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
        public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(AddSubscriptionAsync));

            var result = await _mediator.Send(new GetSubscriptionsRequest(), cancellationToken);
            return result.Subscriptions;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Subscription), (int)HttpStatusCode.OK)]
        [Route("{id:guid}")]
        public async Task<Subscription> GetSubscriptionAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(AddSubscriptionAsync));

            var result = await _mediator.Send(new GetSubscriptionRequest(id), cancellationToken);
            return result.Subscription;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<Subscription> AddSubscriptionAsync([FromQuery] string channelName, [FromQuery] string sourceRepositoryUrl, [FromQuery] string targetRepositoryUrl, [FromQuery] string targetBranchName,
            [FromQuery] UpdateFrequency updateFrequency, [FromQuery] IEnumerable<string> policies, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(AddSubscriptionAsync));

            var result = await _mediator.Send(new AddSubscriptionRequest(channelName, sourceRepositoryUrl, targetRepositoryUrl, targetBranchName, updateFrequency, policies), cancellationToken);
            return result.Subscription;
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("{id:guid}")]
        public async Task<Subscription> EditSubscriptionAsync([FromRoute] Guid id, [FromQuery] string channelName, [FromQuery] string sourceRepositoryUrl, [FromQuery] string targetRepositoryUrl,
            [FromQuery] string targetBranchName, [FromQuery] UpdateFrequency updateFrequency, [FromQuery] IEnumerable<string> policies, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(AddSubscriptionAsync));

            var subscription = new Subscription(channelName, sourceRepositoryUrl, targetRepositoryUrl, targetBranchName, updateFrequency, policies);
            var result = await _mediator.Send(new EditSubscriptionRequest(id, subscription), cancellationToken);
            return result.Subscription;
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("{id:guid}")]
        public async Task RemoveSubscriptionAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(AddSubscriptionAsync));

            _ = await _mediator.Send(new RemoveSubscriptionRequest(id), cancellationToken);
        }
    }
}
