using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Core;
using MediatR;

namespace Conductor.Api.Features.Channels
{
    public record RemoveChannelRequest(string Name) : IRequest<RemoveChannelResponse>;
    public record RemoveChannelResponse(BuildChannel Channel);

    public class RemoveChannelHandler : IRequestHandler<RemoveChannelRequest, RemoveChannelResponse>
    {
        private readonly IConductorService _conductor;

        public RemoveChannelHandler(IConductorService conductor)
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
