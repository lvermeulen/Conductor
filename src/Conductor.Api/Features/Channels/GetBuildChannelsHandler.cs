using System;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using MediatR;

namespace Conductor.Api.Features.Channels
{
    public record GetBuildChannelRequest(string Name) : IRequest<GetBuildChannelResponse>;
    public record GetBuildChannelResponse(BuildChannel Channel);

    public class GetBuildChannelHandler : IRequestHandler<GetBuildChannelRequest, GetBuildChannelResponse>
    {
        private readonly IConductorService _conductor;

        public GetBuildChannelHandler(IConductorService conductor)
        {
            _conductor = conductor;
        }

        public Task<GetBuildChannelResponse> Handle(GetBuildChannelRequest request, CancellationToken cancellationToken)
        {
            foreach ((string key, var value) in _conductor.Channels)
            {
                if (key.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Task.FromResult(new GetBuildChannelResponse(value));
                }
            }

            return Task.FromResult(new GetBuildChannelResponse(default));
        }
    }
}
