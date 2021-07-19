using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Core;
using MediatR;

namespace Conductor.Api.Features.Channels
{
    public record GetChannelsRequest : IRequest<GetChannelsResponse>;
    public record GetChannelsResponse(IEnumerable<BuildChannel> Channels);

    public class GetChannelsHandler : IRequestHandler<GetChannelsRequest, GetChannelsResponse>
    {
        private readonly IConductorService _conductor;

        public GetChannelsHandler(IConductorService conductor)
        {
            _conductor = conductor;
        }

        public Task<GetChannelsResponse> Handle(GetChannelsRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetChannelsResponse(_conductor.Channels.Values));
        }
    }
}
