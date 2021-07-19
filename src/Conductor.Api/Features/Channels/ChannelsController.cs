using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Conductor.Api.Features.Channels
{
    [ApiController]
    [Route("[controller]")]
    public class ChannelsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ChannelsController> _logger;

        public ChannelsController(IMediator mediator, ILogger<ChannelsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BuildChannel>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<BuildChannel>> GetChannelsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(AddChannelAsync));

            var result = await _mediator.Send(new GetChannelsRequest(), cancellationToken);
            return result.Channels;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BuildChannel), (int)HttpStatusCode.OK)]
        [Route("{name}")]
        public async Task<BuildChannel> GetChannelAsync([FromRoute] string name, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(AddChannelAsync));

            var result = await _mediator.Send(new GetChannelRequest(name), cancellationToken);
            return result.Channel;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<BuildChannel> AddChannelAsync([FromQuery] string name, [FromQuery] ClassificationType classificationType, [FromQuery] string repositoryUrl, [FromQuery] string branchName, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(AddChannelAsync));

            var result = await _mediator.Send(new AddChannelRequest(name, classificationType, repositoryUrl, branchName), cancellationToken);
            return result.Channel;
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("{name}")]
        public async Task RemoveChannelAsync([FromRoute] string name, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(AddChannelAsync));

            _ = await _mediator.Send(new RemoveChannelRequest(name), cancellationToken);
        }
    }
}
