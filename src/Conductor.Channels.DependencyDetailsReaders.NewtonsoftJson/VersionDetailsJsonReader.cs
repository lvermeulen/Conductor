using System.IO;
using Newtonsoft.Json.Linq;

namespace Conductor.Channels.DependencyDetailsReaders.NewtonsoftJson
{
	public class VersionDetailsJsonReader : NewtonsoftJsonDependencyDetailsReader
	{
		public static VersionDetailsJsonReader LoadFrom(string fileName)
		{
			if (!File.Exists(fileName))
			{
				return null;
			}

			string s = File.ReadAllText(fileName);
			return Parse(s);
		}

		public static VersionDetailsJsonReader Parse(string s) => new VersionDetailsJsonReader(JObject.Parse(s));

		public VersionDetailsJsonReader(JObject document)
			: base(document)
		{ }

	}
}
