using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using MediatR;

namespace Conductor.Api.Features.Channels
{
    public record AddBuildChannelRequest(string ChannelName, ClassificationType ClassificationType, string RepositoryUrl, string BranchName) : IRequest<AddBuildChannelResponse>;
    public record AddBuildChannelResponse(BuildChannel Channel);

    public class AddBuildChannelHandler : IRequestHandler<AddBuildChannelRequest, AddBuildChannelResponse>
    {
        private readonly IConductorService _conductor;

        public AddBuildChannelHandler(IConductorService conductor)
        {
            _conductor = conductor;
        }

        public async Task<AddBuildChannelResponse> Handle(AddBuildChannelRequest request, CancellationToken cancellationToken)
        {
            var channel = await _conductor.AddBuildChannelAsync(request.ChannelName, request.ClassificationType, request.RepositoryUrl, request.BranchName, cancellationToken);
            return new AddBuildChannelResponse(channel);
        }
    }
}
