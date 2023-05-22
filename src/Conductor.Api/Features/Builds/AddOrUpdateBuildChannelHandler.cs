using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using MediatR;

namespace Conductor.Api.Features.Builds
{
	public record AddOrUpdateBuildChannelRequest(BuildInfo BuildInfo, string RepositoryPath) : IRequest<AddOrUpdateBuildChannelResponse>;

    public record AddOrUpdateBuildChannelResponse(bool AddOrUpdateBuildChannelResult);

	public class AddOrUpdateBuildChannelHandler : IRequestHandler<AddOrUpdateBuildChannelRequest, AddOrUpdateBuildChannelResponse>
	{
		private readonly IConductorService _conductor;

		public AddOrUpdateBuildChannelHandler(IConductorService conductor)
		{
			_conductor = conductor;
		}

		public async Task<AddOrUpdateBuildChannelResponse> Handle(AddOrUpdateBuildChannelRequest request, CancellationToken cancellationToken)
		{
			var result = await _conductor.AddOrUpdateBuildChannelAsync(request.BuildInfo, request.RepositoryPath, cancellationToken);
			return new AddOrUpdateBuildChannelResponse(result);
		}
	}
}
