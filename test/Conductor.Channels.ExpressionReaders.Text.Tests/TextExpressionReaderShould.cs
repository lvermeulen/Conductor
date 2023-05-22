using System.IO;
using Conductor.Abstractions;
using Xunit;

namespace Conductor.Channels.ExpressionReaders.Text.Tests
{
    public class TextExpressionReaderShould
    {
        [Fact]
        public void ReadExpression()
        {
            const string s = "1.2.3";

            const string buildToolsVersion = "BuildToolsVersion";
            const string fileName = buildToolsVersion + ".txt";
            File.WriteAllText(fileName, s);
            try
            {
                IExpressionReader reader = new TextExpressionReader();
                var version = reader.ReadExpression(buildToolsVersion);
                Assert.Equal(s, version);
            }
            finally
            {
                File.Delete(fileName);
            }
        }
    }
}
