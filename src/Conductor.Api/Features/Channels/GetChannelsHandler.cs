using System;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Core;
using MediatR;

namespace Conductor.Api.Features.Channels
{
    public record GetChannelRequest(string Name) : IRequest<GetChannelResponse>;
    public record GetChannelResponse(BuildChannel Channel);

    public class GetChannelHandler : IRequestHandler<GetChannelRequest, GetChannelResponse>
    {
        private readonly IConductorService _conductor;

        public GetChannelHandler(IConductorService conductor)
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
