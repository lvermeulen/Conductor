using System.Threading;
using System.Threading.Tasks;
using Conductor.Core;
using MediatR;

namespace Conductor.Channels.Handlers
{
	public record AddChannelRequest(string ChannelName, ClassificationType ClassificationType, string RepositoryUrl, string BranchName) : IRequest<AddChannelResponse>;
	public record AddChannelResponse(Channel Channel);

	public class AddChannelHandler : IRequestHandler<AddChannelRequest, AddChannelResponse>
	{
		private readonly ConductorService _conductor;

		public AddChannelHandler(ConductorService conductor)
		{
			_conductor = conductor;
		}

		public async Task<AddChannelResponse> Handle(AddChannelRequest request, CancellationToken cancellationToken)
		{
			var channel = await _conductor.AddChannelAsync(request.ChannelName, request.ClassificationType, request.RepositoryUrl, request.BranchName);
			return new AddChannelResponse(channel);
		}
	}
}
