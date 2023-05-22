using System.IO;
using System.Text.Json;

namespace Conductor.Channels.ExpressionReaders.SystemTextJson
{
    public class GlobalJsonExpressionReader : SystemTextJsonExpressionReader
    {
        public static GlobalJsonExpressionReader LoadFrom(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            var s = File.ReadAllText(fileName);
            return Parse(s);
        }

        public static GlobalJsonExpressionReader Parse(string s) => new GlobalJsonExpressionReader(JsonDocument.Parse(s));

        public GlobalJsonExpressionReader(JsonDocument document)
            : base(document)
        { }
    }
}
