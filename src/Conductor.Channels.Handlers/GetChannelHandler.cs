using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Core;
using MediatR;

namespace Conductor.Channels.Handlers
{
	public record GetChannelsRequest : IRequest<GetChannelsResponse>;
	public record GetChannelsResponse(IEnumerable<Channel> Channels);

	public class GetChannelsHandler : IRequestHandler<GetChannelsRequest, GetChannelsResponse>
	{
		private readonly ConductorService _conductor;

		public GetChannelsHandler(ConductorService conductor)
		{
			_conductor = conductor;
		}

		public Task<GetChannelsResponse> Handle(GetChannelsRequest request, CancellationToken cancellationToken)
		{
			return Task.FromResult(new GetChannelsResponse(_conductor.Channels.Values));
		}
	}
}
