using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Api.Features.Channels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Conductor.Api.Features.Builds
{
	[ApiController]
	[Route("[controller]")]
	public class BuildsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<ChannelsController> _logger;

		public BuildsController(IMediator mediator, ILogger<ChannelsController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpPost]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<bool> AddOrUpdateBuildChannelAsync([FromBody] BuildInfo buildInfo, CancellationToken cancellationToken = default)
		{
			_logger.LogTrace(nameof(AddOrUpdateBuildChannelAsync));

			var result = await _mediator.Send(new AddOrUpdateBuildChannelRequest(buildInfo), cancellationToken);
			return result.AddOrUpdateBuildChannelResult;
		}
	}
}
