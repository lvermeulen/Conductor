using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Core;
using Xunit;

namespace Conductor.Api.Tests.Features.Builds
{
	public class AddOrUpdateBuildChannelHandlerShould
	{
		private readonly IConductorService _conductor = new ConductorBuilder().Build();

		[Fact]
		public async Task Handle()
		{
			var channel = await _conductor.AddChannelAsync(nameof(Handle), ClassificationType.Product, "http://some.repo.url", "main");
			bool result = await _conductor.AddOrUpdateBuildChannelAsync(new BuildInfo
			{
				SourceRepository = "http://some.source.repo.url",
				ChannelName = channel.Name,
				ArtifactsUrl = "http://some.artifacts.url"
			});
			Assert.True(result);
		}
	}
}
