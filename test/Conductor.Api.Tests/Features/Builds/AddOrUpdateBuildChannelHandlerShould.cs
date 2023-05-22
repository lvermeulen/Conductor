using System.IO;
using System.Threading.Tasks;
using Conductor.Abstractions;
using Conductor.Core;
using Conductor.Tests;
using Xunit;

namespace Conductor.Api.Tests.Features.Builds
{
	public class AddOrUpdateBuildChannelHandlerShould
	{
		private readonly IConductorService _conductor = new ConductorBuilder().Build();

		[Fact]
		public async Task Handle()
		{
			var fileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			var dir = Directory.CreateDirectory(fileName);
			var repositoryPath = dir.FullName;

			using (new AutoCleanupFolder(repositoryPath))
			{
				var channel = await _conductor.AddBuildChannelAsync(nameof(Handle), ClassificationType.Product, "http://some.repo.url", "main");
				var result = await _conductor.AddOrUpdateBuildChannelAsync(new BuildInfo("http://some.source.repo.url", channel.Name, "http://some.artifacts.url"), repositoryPath);
				Assert.True(result);
			}

			Assert.False(Directory.Exists(repositoryPath));
		}
	}
}
