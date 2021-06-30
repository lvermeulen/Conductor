using System.Threading;
using System.Threading.Tasks;
using Conductor.Core;
using MediatR;

namespace Conductor.Channels.Handlers
{
	public record RemoveChannelRequest(string Name) : IRequest<RemoveChannelResponse>;
	public record RemoveChannelResponse(Channel Channel);

	public class RemoveChannelHandler : IRequestHandler<RemoveChannelRequest, RemoveChannelResponse>
	{
		private readonly ConductorService _conductor;

		public RemoveChannelHandler(ConductorService conductor)
		{
			_conductor = conductor;
		}

		public async Task<RemoveChannelResponse> Handle(RemoveChannelRequest request, CancellationToken cancellationToken)
		{
			var channel = await _conductor.FindChannelByNameAsync(request.Name);
			await _conductor.RemoveChannelAsync(request.Name);
			return new RemoveChannelResponse(channel);
		}
	}
}
