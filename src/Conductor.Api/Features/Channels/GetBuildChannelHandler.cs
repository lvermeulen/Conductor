using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using MediatR;

namespace Conductor.Api.Features.Channels
{
    public record GetBuildChannelsRequest : IRequest<GetBuildChannelsResponse>;
    public record GetBuildChannelsResponse(IEnumerable<BuildChannel> Channels);

    public class GetBuildChannelsHandler : IRequestHandler<GetBuildChannelsRequest, GetBuildChannelsResponse>
    {
        private readonly IConductorService _conductor;

        public GetBuildChannelsHandler(IConductorService conductor)
        {
            _conductor = conductor;
        }

        public Task<GetBuildChannelsResponse> Handle(GetBuildChannelsRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetBuildChannelsResponse(_conductor.Channels.Values));
        }
    }
}
