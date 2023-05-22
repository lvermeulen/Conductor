using System.IO;
using System.Linq;
using Conductor.Abstractions;
using Xunit;

namespace Conductor.Channels.DependencyDetailsReaders.NewtonsoftJson.Tests
{
    public class VersionDetailsJsonReaderShould
    {
        private const string Text = @"{
	""Dependencies"": {
		""ProductDependencies"": [{
			""Name"": ""DependencyA"",
			""Version"": ""1.2.3-45"",
			""Pinned"": true,
			""Uri"": ""https://github.com/dotnet/arepo"",
			""Sha"": ""23498123740982349182340981234""
		},
		{
			""Name"": ""DependencyB"",
			""Version"": ""1.2.3-45"",
			""Uri"": ""https://github.com/dotnet/arepo"",
			""Sha"": ""13242134123412341465""
		},
		{
			""Name"": ""DependencyC"",
			""Version"": ""1.2.3-45"",
			""Pinned"": false,
			""Uri"": ""https://github.com/dotnet/arepo"",
			""Sha"": ""789789789789789789789789""
		}],
		""ToolsetDependencies"": [{
			""Name"": ""DependencyD"",
			""Version"": ""2.100.3-1234"",
			""Uri"": ""https://github.com/dotnet/atoolsrepo"",
			""Sha"": ""203409823586523490823498234"",
			""Expression"": ""VersionProps""
		},
		{
			""Name"": ""DotNetSdkVersion"",
			""Version"": ""2.200.0"",
			""Pinned"": false,
			""Uri"": ""https://github.com/dotnet/cli"",
			""Sha"": ""1234123412341234""
		},
		{
			""Name"": ""Arcade.Sdk"",
			""Version"": ""1.0.0"",
			""Uri"": ""https://github.com/dotnet/arcade"",
			""Sha"": ""132412342341234234""
		}]
	}
}";

        [Fact]
        public void LoadFrom()
        {
            // create file
            var fileName = $"{nameof(NewtonsoftJson)}-{nameof(VersionDetailsJsonReaderShould)}-{nameof(LoadFrom)}.txt";
            File.WriteAllText(fileName, Text);
            try
            {
                IDependencyDetailsReader reader = VersionDetailsJsonReader.LoadFrom(fileName);
                var result = reader.ReadMetadata();
                Assert.NotNull(result);
                Assert.NotNull(result.Dependencies);
                Assert.Equal(6, result.Dependencies.Count());
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Fact]
        public void Parse()
        {
            IDependencyDetailsReader reader = VersionDetailsJsonReader.Parse(Text);
            var result = reader.ReadMetadata();
            Assert.NotNull(result);
            Assert.NotNull(result.Dependencies);
            Assert.Equal(6, result.Dependencies.Count());
        }
    }
}
