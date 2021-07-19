using System.Linq;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Api.Features.Channels;
using Conductor.Api.Features.Subscriptions;
using Conductor.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Conductor.Api.Tests.Features.Subscriptions
{
    public class SubscriptionsControllerShould : BaseConductorTest<Startup>
    {
        private readonly SubscriptionsController _controller;

        public SubscriptionsControllerShould(ConductorWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
            : base(factory, testOutputHelper)
        {
            _controller = Factory.Services.GetRequiredService<SubscriptionsController>();
        }

        [Fact]
        public async Task GetSubscriptionsAsync()
        {
            var result = await _controller.GetSubscriptionsAsync();
            Assert.NotNull(result);
            foreach (var subscription in result)
            {
                TestOutputHelper.WriteLine($"Subscription: {subscription}");
            }
        }

        [Fact]
        public async Task GetSubscriptionAsync()
        {
            var results = await _controller.GetSubscriptionsAsync();
            var firstResult = results.FirstOrDefault();
            if (firstResult is null)
            {
                return;
            }

            var result = await _controller.GetSubscriptionAsync(firstResult.Id);
            Assert.NotNull(result);
            TestOutputHelper.WriteLine($"Subscription: {result}");
        }

        [Fact]
        public async Task GetAddEditRemoveSubscriptionAsync()
        {
            var channelsController = Factory.Services.GetRequiredService<ChannelsController>();

            var result = await _controller.GetSubscriptionsAsync();
            Assert.NotNull(result);
            Assert.Empty(result);

            var channel = await channelsController.AddChannelAsync(nameof(GetAddEditRemoveSubscriptionAsync), ClassificationType.Product, "http://some.url", "feature/some-feature");
            var subscription = await _controller.AddSubscriptionAsync(channel.Name, channel.RepositoryUrl, "http://some.target.url", "feature/some-target-feature", UpdateFrequency.None, Enumerable.Empty<string>());

            result = await _controller.GetSubscriptionsAsync();
            Assert.NotNull(result);
            Assert.Single(result);

            var one = await _controller.GetSubscriptionAsync(subscription.Id);
            Assert.NotNull(one);
            Assert.Equal(subscription.Id, one.Id);

            const string targetBranchName = "feature/some-other-target-feature";
            subscription = await _controller.EditSubscriptionAsync(subscription.Id, subscription.ChannelName, subscription.SourceRepositoryUrl, subscription.TargetRepositoryUrl, targetBranchName,
                subscription.UpdateFrequency, subscription.Policies);

            result = await _controller.GetSubscriptionsAsync();
            Assert.NotNull(result);
            Assert.Equal(targetBranchName, result.FirstOrDefault()?.TargetBranchName);

            await _controller.RemoveSubscriptionAsync(subscription.Id);

            result = await _controller.GetSubscriptionsAsync();
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
