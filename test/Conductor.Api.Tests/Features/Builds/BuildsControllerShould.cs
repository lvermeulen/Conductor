using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Api.Features.Builds;
using Conductor.Api.Features.Channels;
using Conductor.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Conductor.Api.Tests.Features.Builds
{
	public class BuildsControllerShould : BaseConductorTest<Startup>
	{
		private readonly BuildsController _controller;
		private readonly BuildChannelsController _channelsController;

		public BuildsControllerShould(ConductorWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
			: base(factory, testOutputHelper)
		{
            _controller = Factory.Services.GetRequiredService<BuildsController>();
            _channelsController = Factory.Services.GetRequiredService<BuildChannelsController>();
		}

        [Fact]
        public async Task AddOrUpdateBuildChannelAsync()
        {
	        var channel = await _channelsController.AddBuildChannelAsync(nameof(AddOrUpdateBuildChannelAsync), ClassificationType.Product, "http://some.repo.url", "main", CancellationToken.None);
	        bool result = await _controller.AddOrUpdateBuildChannelAsync(new BuildInfo("http://some.url", channel.Name, "http://some.artifact.url"), CancellationToken.None);
            Assert.True(result);
        }
	}
}
