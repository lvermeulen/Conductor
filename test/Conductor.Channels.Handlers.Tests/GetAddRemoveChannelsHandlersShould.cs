using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Core;
using Xunit;

namespace Conductor.Channels.Handlers.Tests
{
	public class GetAddRemoveChannelsHandlersShould
	{
		private readonly ConductorService _conductor = new ConductorBuilder().Build();

		[Fact]
		public async Task Handle()
		{
			_ = await _conductor.AddChannelAsync(nameof(GetAddRemoveChannelsHandlersShould), ClassificationType.Product, "http://some.url", "feature/some-feature");

			var getHandler = new GetChannelsHandler(_conductor);
			var addHandler = new AddChannelHandler(_conductor);
			var removeHandler = new RemoveChannelHandler(_conductor);

			var getResult = await getHandler.Handle(new GetChannelsRequest(), CancellationToken.None);
			Assert.NotNull(getResult);
			Assert.NotNull(getResult.Channels);
			Assert.Single(getResult.Channels);

			// get one
			var firstChannel = getResult.Channels.FirstOrDefault();
			if (firstChannel is not null)
			{
				var getOneHandler = new GetChannelHandler(_conductor);
				var getOneResult = await getOneHandler.Handle(new GetChannelRequest(firstChannel.Name), CancellationToken.None);
				Assert.NotNull(getOneResult);
				Assert.NotNull(getOneResult.Channel);
				Assert.Equal(firstChannel.Name, getOneResult.Channel.Name);
			}

			// add
			_ = await addHandler.Handle(new AddChannelRequest(nameof(Handle), ClassificationType.Product, "http://some.other.url", "feature/some-feature"), CancellationToken.None);
			getResult = await getHandler.Handle(new GetChannelsRequest(), CancellationToken.None);
			Assert.NotNull(getResult);
			Assert.NotNull(getResult.Channels);
			Assert.Equal(2, getResult.Channels.Count());

			var channel = getResult.Channels.Last();
			_ = await removeHandler.Handle(new RemoveChannelRequest(channel.Name), CancellationToken.None);
			getResult = await getHandler.Handle(new GetChannelsRequest(), CancellationToken.None);
			Assert.NotNull(getResult);
			Assert.NotNull(getResult.Channels);
			Assert.Single(getResult.Channels);
		}
	}
}
