using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Api.Features.Channels;
using Conductor.Core;
using Xunit;

namespace Conductor.Api.Tests.Features.Channels
{
    public class GetAddRemoveBuildChannelsHandlerShould
    {
        private readonly IConductorService _conductor = new ConductorBuilder().Build();

        [Fact]
        public async Task Handle()
        {
            _ = await _conductor.AddBuildChannelAsync(nameof(GetAddRemoveBuildChannelsHandlerShould), ClassificationType.Product, "http://some.url", "feature/some-feature");

            var getHandler = new GetBuildChannelsHandler(_conductor);
            var addHandler = new AddBuildChannelHandler(_conductor);
            var removeHandler = new RemoveBuildChannelHandler(_conductor);

            var getResult = await getHandler.Handle(new GetBuildChannelsRequest(), CancellationToken.None);
            Assert.NotNull(getResult);
            Assert.NotNull(getResult.Channels);
            Assert.Single(getResult.Channels);

            // get one
            var firstChannel = getResult.Channels.FirstOrDefault();
            if (firstChannel is not null)
            {
                var getOneHandler = new GetBuildChannelHandler(_conductor);
                var getOneResult = await getOneHandler.Handle(new GetBuildChannelRequest(firstChannel.Name), CancellationToken.None);
                Assert.NotNull(getOneResult);
                Assert.NotNull(getOneResult.Channel);
                Assert.Equal(firstChannel.Name, getOneResult.Channel.Name);
            }

            // add
            _ = await addHandler.Handle(new AddBuildChannelRequest(nameof(Handle), ClassificationType.Product, "http://some.other.url", "feature/some-feature"), CancellationToken.None);
            getResult = await getHandler.Handle(new GetBuildChannelsRequest(), CancellationToken.None);
            Assert.NotNull(getResult);
            Assert.NotNull(getResult.Channels);
            Assert.Equal(2, getResult.Channels.Count());

            var channel = getResult.Channels.Last();
            _ = await removeHandler.Handle(new RemoveBuildChannelRequest(channel.Name), CancellationToken.None);
            getResult = await getHandler.Handle(new GetBuildChannelsRequest(), CancellationToken.None);
            Assert.NotNull(getResult);
            Assert.NotNull(getResult.Channels);
            Assert.Single(getResult.Channels);
        }
    }
}
