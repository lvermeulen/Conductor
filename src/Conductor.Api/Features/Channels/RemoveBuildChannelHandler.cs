using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using MediatR;

namespace Conductor.Api.Features.Channels
{
    public record RemoveBuildChannelRequest(string Name) : IRequest<RemoveBuildChannelResponse>;
    public record RemoveBuildChannelResponse(BuildChannel Channel);

    public class RemoveBuildChannelHandler : IRequestHandler<RemoveBuildChannelRequest, RemoveBuildChannelResponse>
    {
        private readonly IConductorService _conductor;

        public RemoveBuildChannelHandler(IConductorService conductor)
        {
            _conductor = conductor;
        }

        public async Task<RemoveBuildChannelResponse> Handle(RemoveBuildChannelRequest request, CancellationToken cancellationToken)
        {
            var channel = await _conductor.FindBuildChannelByNameAsync(request.Name, cancellationToken);
            await _conductor.RemoveBuildChannelAsync(request.Name, cancellationToken);
            return new RemoveBuildChannelResponse(channel);
        }
    }
}
