using Xunit;
using Xunit.Abstractions;

namespace Conductor.Tests
{
	public abstract class BaseConductorTest<TStartup> : IClassFixture<ConductorWebApplicationFactory<TStartup>>
		where TStartup : class
	{
		protected readonly ConductorWebApplicationFactory<TStartup> Factory;
		protected readonly ITestOutputHelper TestOutputHelper;

		protected BaseConductorTest(ConductorWebApplicationFactory<TStartup> factory, ITestOutputHelper testOutputHelper)
		{
			Factory = factory;
			TestOutputHelper = testOutputHelper;
		}
	}
}
