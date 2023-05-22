using System.IO;
using Newtonsoft.Json.Linq;

namespace Conductor.Channels.ExpressionReaders.NewtonsoftJson
{
    public class VersionPropsJsonExpressionReader : NewtonsoftJsonExpressionReader
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

        public static VersionPropsJsonExpressionReader Parse(string s) => new VersionPropsJsonExpressionReader(JObject.Parse(s));

        public VersionPropsJsonExpressionReader(JObject document)
            : base(document)
        { }
    }
}
