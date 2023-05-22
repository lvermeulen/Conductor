using System.IO;
using Conductor.Abstractions;
using Xunit;

namespace Conductor.Channels.ExpressionReaders.SystemTextJson.Tests
{
    public class VersionPropsJsonExpressionReaderShould
    {
        private const string Text = @"{
  ""PropertyGroup"": {
    ""MicrosoftExtensionsDependencyModelPackageVersion"": ""2.1.0-preview2-26314-02""
  }
}";

        [Fact]
        public void LoadFrom()
        {
            // create file
            var fileName = $"{nameof(SystemTextJson)}-{nameof(VersionPropsJsonExpressionReaderShould)}-{nameof(LoadFrom)}.txt";
            File.WriteAllText(fileName, Text);
            try
            {
                IExpressionReader reader = VersionPropsJsonExpressionReader.LoadFrom(fileName);
                var version = reader.ReadExpression("MicrosoftExtensionsDependencyModelPackageVersion");
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
            IExpressionReader reader = VersionPropsJsonExpressionReader.Parse(Text);
            var version = reader.ReadExpression("MicrosoftExtensionsDependencyModelPackageVersion");
            Assert.Equal("2.1.0-preview2-26314-02", version);
        }
    }
}
