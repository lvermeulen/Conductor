using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Conductor.Channels;
using Conductor.Channels.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Conductor.Api.Controllers
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
		[ProducesResponseType(typeof(IEnumerable<Channel>), (int)HttpStatusCode.OK)]
		public async Task<IEnumerable<Channel>> GetChannelsAsync()
		{
			_logger.LogTrace(nameof(AddChannelAsync));

			var result = await _mediator.Send(new GetChannelsRequest());
			return result.Channels;
		}

		[HttpGet]
		[ProducesResponseType(typeof(Channel), (int)HttpStatusCode.OK)]
		[Route("{name}")]
		public async Task<Channel> GetChannelAsync([FromRoute] string name)
		{
			_logger.LogTrace(nameof(AddChannelAsync));

			var result = await _mediator.Send(new GetChannelRequest(name));
			return result.Channel;
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<Channel> AddChannelAsync([FromQuery] string name, [FromQuery] ClassificationType classificationType, [FromQuery] string repositoryUrl, [FromQuery] string branchName)
		{
			_logger.LogTrace(nameof(AddChannelAsync));

			var result = await _mediator.Send(new AddChannelRequest(name, classificationType, repositoryUrl, branchName));
			return result.Channel;
		}

		[HttpDelete]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[Route("{name}")]
		public async Task RemoveChannelAsync([FromRoute] string name)
		{
			_logger.LogTrace(nameof(AddChannelAsync));

			_ = await _mediator.Send(new RemoveChannelRequest(name));
		}
	}
}
