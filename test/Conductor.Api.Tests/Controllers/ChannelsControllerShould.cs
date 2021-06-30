using System.Linq;
using System.Threading.Tasks;
using Conductor.Api.Controllers;
using Conductor.Channels;
using Conductor.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Conductor.Api.Tests.Controllers
{
	public class ChannelsControllerShould : BaseConductorTest<Startup>
	{
		private readonly ChannelsController _controller;

		public ChannelsControllerShould(ConductorWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
			: base(factory, testOutputHelper)
		{
			_controller = Factory.Services.GetRequiredService<ChannelsController>();
		}

		[Fact]
		public async Task GetChannelsAsync()
		{
			var result = await _controller.GetChannelsAsync();
			Assert.NotNull(result);
			foreach (var channel in result)
			{
				TestOutputHelper.WriteLine($"Channel: {channel}");
			}
		}

		[Fact]
		public async Task GetChannelAsync()
		{
			var results = await _controller.GetChannelsAsync();
			var firstResult = results.FirstOrDefault();
			if (firstResult is null)
			{
				return;
			}

			var result = await _controller.GetChannelAsync(firstResult.Name);
			Assert.NotNull(result);
			TestOutputHelper.WriteLine($"Channel: {result}");
		}

		[Fact]
		public async Task GetAddRemoveChannelAsync()
		{
			var result = await _controller.GetChannelsAsync();
			Assert.NotNull(result);
			Assert.Empty(result);

			var channel = await _controller.AddChannelAsync(nameof(GetAddRemoveChannelAsync), ClassificationType.Product, "http://some.target.url", "feature/some-feature");

			result = await _controller.GetChannelsAsync();
			Assert.NotNull(result);
			Assert.Single(result);

			var one = await _controller.GetChannelAsync(channel.Name);
			Assert.NotNull(one);
			Assert.Equal(channel.Name, one.Name);

			await _controller.RemoveChannelAsync(channel.Name);

			result = await _controller.GetChannelsAsync();
			Assert.NotNull(result);
			Assert.Empty(result);
		}
	}
}
