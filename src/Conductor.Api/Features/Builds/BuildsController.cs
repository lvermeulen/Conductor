using System.IO;
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
		private readonly ILogger<BuildChannelsController> _logger;

		public BuildsController(IMediator mediator, ILogger<BuildChannelsController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpPost]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<bool> AddOrUpdateBuildChannelAsync([FromBody] BuildInfo buildInfo, CancellationToken cancellationToken = default)
		{
			_logger.LogTrace(nameof(AddOrUpdateBuildChannelAsync));

			var fileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			var dir = Directory.CreateDirectory(fileName);
			var repositoryPath = dir.FullName;

			var result = await _mediator.Send(new AddOrUpdateBuildChannelRequest(buildInfo, repositoryPath), cancellationToken);
			return result.AddOrUpdateBuildChannelResult;
		}
	}
}
