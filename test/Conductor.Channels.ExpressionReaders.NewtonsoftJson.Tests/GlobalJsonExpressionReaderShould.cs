using System.IO;
using Conductor.Abstractions;
using Xunit;

namespace Conductor.Channels.ExpressionReaders.NewtonsoftJson.Tests
{
    public class GlobalJsonExpressionReaderShould
    {
        private const string Text = @"{
	""sdk"": {
		""version"": ""2.200.0""
	},
	""msbuild-sdks"": {
		""Arcade.Sdk"": ""1.0.0""
	}
}";

        [Fact]
        public void LoadFrom()
        {
            // create file
            var fileName = $"{nameof(NewtonsoftJson)}-{nameof(GlobalJsonExpressionReaderShould)}-{nameof(LoadFrom)}.txt";
            File.WriteAllText(fileName, Text);
            try
            {
                IExpressionReader reader = GlobalJsonExpressionReader.LoadFrom(fileName);
                var version = reader.ReadExpression("Arcade.Sdk");
                Assert.Equal("1.0.0", version);
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Fact]
        public void Parse()
        {
            IExpressionReader reader = GlobalJsonExpressionReader.Parse(Text);
            var version = reader.ReadExpression("Arcade.Sdk");
            Assert.Equal("1.0.0", version);
        }
    }
}
