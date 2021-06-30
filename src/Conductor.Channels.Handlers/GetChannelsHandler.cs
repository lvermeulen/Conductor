using System;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Core;
using MediatR;

namespace Conductor.Channels.Handlers
{
	public record GetChannelRequest(string Name) : IRequest<GetChannelResponse>;
	public record GetChannelResponse(Channel Channel);

	public class GetChannelHandler : IRequestHandler<GetChannelRequest, GetChannelResponse>
	{
		private readonly ConductorService _conductor;

		public GetChannelHandler(ConductorService conductor)
		{
			_conductor = conductor;
		}

		public Task<GetChannelResponse> Handle(GetChannelRequest request, CancellationToken cancellationToken)
		{
			foreach ((string key, var value) in _conductor.Channels)
			{
				if (key.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase))
				{
					return Task.FromResult(new GetChannelResponse(value));
				}
			}

			return Task.FromResult(new GetChannelResponse(default));
		}
	}
}
