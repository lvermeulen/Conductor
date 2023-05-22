using System.IO;
using System.Text.Json;

namespace Conductor.Channels.DependencyDetailsReaders.SystemTextJson
{
    public class VersionDetailsJsonReader : SystemTextJsonDependencyDetailsReader
    {
        public static VersionDetailsJsonReader LoadFrom(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            var s = File.ReadAllText(fileName);
            return Parse(s);
        }

        public static VersionDetailsJsonReader Parse(string s) => new VersionDetailsJsonReader(JsonDocument.Parse(s));

        public VersionDetailsJsonReader(JsonDocument document)
            : base(document)
        { }
    }
}
