using System.IO;
using System.Text.Json;

namespace Conductor.Channels.ExpressionReaders.SystemTextJson
{
    public class VersionPropsJsonExpressionReader : SystemTextJsonExpressionReader
    {
        public static VersionPropsJsonExpressionReader LoadFrom(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            var s = File.ReadAllText(fileName);
            return Parse(s);
        }

        public static VersionPropsJsonExpressionReader Parse(string s) => new VersionPropsJsonExpressionReader(JsonDocument.Parse(s));

        public VersionPropsJsonExpressionReader(JsonDocument document)
            : base(document)
        { }
    }
}
