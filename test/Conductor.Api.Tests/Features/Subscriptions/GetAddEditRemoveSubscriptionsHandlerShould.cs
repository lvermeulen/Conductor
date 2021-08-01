using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Api.Features.Subscriptions;
using Conductor.Core;
using Xunit;

namespace Conductor.Api.Tests.Features.Subscriptions
{
    public class GetAddEditRemoveSubscriptionsHandlerShould
    {
        private readonly IConductorService _conductor = new ConductorBuilder().Build();

        [Fact]
        public async Task Handle()
        {
            var channel = await _conductor.AddBuildChannelAsync(nameof(GetAddEditRemoveSubscriptionsHandlerShould), ClassificationType.Product, "http://some.url", "feature/some-feature");
            _ = await channel.AddSubscriptionAsync("http://some.url", "http://target1.url", "feature/target1", UpdateFrequency.Daily, Enumerable.Empty<string>(), CancellationToken.None);
            _ = await channel.AddSubscriptionAsync("http://some.url", "http://target2.url", "feature-target2", UpdateFrequency.Daily, Enumerable.Empty<string>(), CancellationToken.None);

            var getHandler = new GetSubscriptionsHandler(_conductor);
            var addHandler = new AddSubscriptionHandler(_conductor);
            var editHandler = new EditSubscriptionHandler(_conductor);
            var removeHandler = new RemoveSubscriptionHandler(_conductor);

            var getResult = await getHandler.Handle(new GetSubscriptionsRequest(), CancellationToken.None);
            Assert.NotNull(getResult);
            Assert.NotNull(getResult.Subscriptions);
            Assert.Equal(2, getResult.Subscriptions.Count());

            // get one
            var firstSubscription = getResult.Subscriptions.FirstOrDefault();
            if (firstSubscription is not null)
            {
                var getOneHandler = new GetSubscriptionHandler(_conductor);
                var getOneResult = await getOneHandler.Handle(new GetSubscriptionRequest(firstSubscription.Id), CancellationToken.None);
                Assert.NotNull(getOneResult);
                Assert.NotNull(getOneResult.Subscription);
                Assert.Equal(firstSubscription.Id, getOneResult.Subscription.Id);
            }

            // add
            _ = await addHandler.Handle(new AddSubscriptionRequest(channel.Name, "http://some.url", "http://target3.url", "feature/target3", UpdateFrequency.None, Enumerable.Empty<string>()), CancellationToken.None);
            getResult = await getHandler.Handle(new GetSubscriptionsRequest(), CancellationToken.None);
            Assert.NotNull(getResult);
            Assert.NotNull(getResult.Subscriptions);
            Assert.Equal(3, getResult.Subscriptions.Count());

            var subscription = getResult.Subscriptions.Last();
            const string targetBranchName = "feature/target33";
            var subscription2 = subscription with { TargetBranchName = targetBranchName };
            _ = await editHandler.Handle(new EditSubscriptionRequest(subscription2.Id, subscription2), CancellationToken.None);
            getResult = await getHandler.Handle(new GetSubscriptionsRequest(), CancellationToken.None);
            Assert.NotNull(getResult);
            Assert.NotNull(getResult.Subscriptions);
            subscription2 = getResult.Subscriptions.Last();
            Assert.Equal(targetBranchName, subscription2.TargetBranchName);

            _ = await removeHandler.Handle(new RemoveSubscriptionRequest(subscription.Id), CancellationToken.None);
            getResult = await getHandler.Handle(new GetSubscriptionsRequest(), CancellationToken.None);
            Assert.NotNull(getResult);
            Assert.NotNull(getResult.Subscriptions);
            Assert.Equal(2, getResult.Subscriptions.Count());
        }
    }
}
