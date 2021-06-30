using System.IO;
using System.Linq;
using Conductor.Abstractions;
using Xunit;

namespace Conductor.Channels.DependencyDetailsReaders.Xml.Tests
{
	public class VersionDetailsXmlReaderShould
	{
		const string text = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dependencies>

	<!-- Elements contains all product dependencies -->
	<ProductDependencies>
		<!-- All product dependencies are contained in Version.Props -->
		<Dependency Name=""DependencyA"" Version=""1.2.3-45"" Pinned=""true"">
			<Uri>https://github.com/dotnet/arepo</Uri>
			<Sha>23498123740982349182340981234</Sha>
		</Dependency>
		<Dependency Name=""DependencyB"" Version=""1.2.3-45"">
			<Uri>https://github.com/dotnet/arepo</Uri>
			<Sha>13242134123412341465</Sha>
		</Dependency>
		<Dependency Name=""DependencyC"" Version=""1.2.3-45"" Pinned=""false"">
			<Uri>https://github.com/dotnet/arepo</Uri>
			<Sha>789789789789789789789789</Sha>
		</Dependency>
	</ProductDependencies>

	<!-- Elements contains all toolset dependencies -->
	<ToolsetDependencies>
		<!-- Non well-known dependency.  Expressed in Version.props -->
		<Dependency Name=""DependencyD"" Version=""2.100.3-1234"">
			<Uri>https://github.com/dotnet/atoolsrepo</Uri>
			<Sha>203409823586523490823498234</Sha>
			<Expression>VersionProps</Expression>
		</Dependency>

		<!-- Well-known dependency.  Expressed in global.json -->
		<Dependency Name=""DotNetSdkVersion"" Version=""2.200.0"" Pinned=""False"">
			<Uri>https://github.com/dotnet/cli</Uri>
			<Sha>1234123412341234</Sha>
		</Dependency>

		<!-- Well-known dependency.  Expressed in global.json -->
		<Dependency Name=""Arcade.Sdk"" Version=""1.0.0"">
			<Uri>https://github.com/dotnet/arcade</Uri>
			<Sha>132412342341234234</Sha>
		</Dependency>
	</ToolsetDependencies>
</Dependencies>";
		
		[Fact]
		public void LoadFrom()
		{
			// create file
			string fileName = nameof(LoadFrom) + ".txt";
			File.WriteAllText(fileName, text);
			try
			{
				IDependencyDetailsReader reader = VersionDetailsXmlReader.LoadFrom(fileName);
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
		public void ReadMetadata()
		{
			IDependencyDetailsReader reader = VersionDetailsXmlReader.Parse(text);
			var result = reader.ReadMetadata();
			Assert.NotNull(result);
			Assert.NotNull(result.Dependencies);
			Assert.Equal(6, result.Dependencies.Count());
		}
	}
}
