using System.Linq;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Api.Features.Channels;
using Conductor.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Conductor.Api.Tests.Features.Channels
{
    public class BuildChannelsControllerShould : BaseConductorTest<Startup>
    {
        private readonly BuildChannelsController _controller;

        public BuildChannelsControllerShould(ConductorWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
            : base(factory, testOutputHelper)
        {
            _controller = Factory.Services.GetRequiredService<BuildChannelsController>();
        }

        [Fact]
        public async Task GetChannelsAsync()
        {
            var result = await _controller.GetBuildChannelsAsync();
            Assert.NotNull(result);
            foreach (var channel in result)
            {
                TestOutputHelper.WriteLine($"Channel: {channel}");
            }
        }

        [Fact]
        public async Task GetChannelAsync()
        {
            var results = await _controller.GetBuildChannelsAsync();
            var firstResult = results.FirstOrDefault();
            if (firstResult is null)
            {
                return;
            }

            var result = await _controller.GetBuildChannelAsync(firstResult.Name);
            Assert.NotNull(result);
            TestOutputHelper.WriteLine($"Channel: {result}");
        }

        [Fact]
        public async Task GetAddRemoveChannelAsync()
        {
            var result = await _controller.GetBuildChannelsAsync();
            Assert.NotNull(result);
            Assert.Empty(result);

            var channel = await _controller.AddBuildChannelAsync(nameof(GetAddRemoveChannelAsync), ClassificationType.Product, "http://some.target.url", "feature/some-feature");

            result = await _controller.GetBuildChannelsAsync();
            Assert.NotNull(result);
            Assert.Single(result);

            var one = await _controller.GetBuildChannelAsync(channel.Name);
            Assert.NotNull(one);
            Assert.Equal(channel.Name, one.Name);

            await _controller.RemoveChannelAsync(channel.Name);

            result = await _controller.GetBuildChannelsAsync();
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
