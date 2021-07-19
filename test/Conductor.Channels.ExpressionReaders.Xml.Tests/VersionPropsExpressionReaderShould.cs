using System.IO;
using Conductor.Abstractions;
using Xunit;

namespace Conductor.Channels.ExpressionReaders.Xml.Tests
{
    public class VersionPropsExpressionReaderShould
    {
        private const string text = @"<PropertyGroup>
	<MicrosoftExtensionsDependencyModelPackageVersion>2.1.0-preview2-26314-02</MicrosoftExtensionsDependencyModelPackageVersion>
</PropertyGroup>";

        [Fact]
        public void LoadFrom()
        {
            // create file
            string fileName = $"{nameof(Xml)}-{nameof(VersionPropsExpressionReaderShould)}-{nameof(LoadFrom)}.txt";
            File.WriteAllText(fileName, text);
            try
            {
                IExpressionReader reader = VersionPropsExpressionReader.LoadFrom(fileName);
                string version = reader.ReadExpression("MicrosoftExtensionsDependencyModelPackageVersion");
                Assert.Equal("2.1.0-preview2-26314-02", version);
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Fact]
        public void Parse()
        {
            IExpressionReader reader = VersionPropsExpressionReader.Parse(text);
            string version = reader.ReadExpression("MicrosoftExtensionsDependencyModelPackageVersion");
            Assert.Equal("2.1.0-preview2-26314-02", version);
        }
    }
}
