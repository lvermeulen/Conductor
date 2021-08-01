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
    public class BuildChannelsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BuildChannelsController> _logger;

        public BuildChannelsController(IMediator mediator, ILogger<BuildChannelsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BuildChannel>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<BuildChannel>> GetBuildChannelsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(GetBuildChannelsAsync));

            var result = await _mediator.Send(new GetBuildChannelsRequest(), cancellationToken);
            return result.Channels;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BuildChannel), (int)HttpStatusCode.OK)]
        [Route("{name}")]
        public async Task<BuildChannel> GetBuildChannelAsync([FromRoute] string name, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(GetBuildChannelAsync));

            var result = await _mediator.Send(new GetBuildChannelRequest(name), cancellationToken);
            return result.Channel;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<BuildChannel> AddBuildChannelAsync([FromQuery] string name, [FromQuery] ClassificationType classificationType, [FromQuery] string repositoryUrl, [FromQuery] string branchName, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(AddBuildChannelAsync));

            var result = await _mediator.Send(new AddBuildChannelRequest(name, classificationType, repositoryUrl, branchName), cancellationToken);
            return result.Channel;
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("{name}")]
        public async Task RemoveChannelAsync([FromRoute] string name, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace(nameof(RemoveChannelAsync));

            _ = await _mediator.Send(new RemoveBuildChannelRequest(name), cancellationToken);
        }
    }
}
