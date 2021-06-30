using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Conductor.Channels.ExpressionReaders.NewtonsoftJson
{
	public class GlobalJsonExpressionReader : NewtonsoftJsonExpressionReader
	{
		public static GlobalJsonExpressionReader LoadFrom(string fileName)
		{
			using var stm = new FileStream(fileName, FileMode.Open);
			using var stmReader = new StreamReader(stm);
			using var reader = new JsonTextReader(stmReader);
			return new GlobalJsonExpressionReader(JObject.Load(reader));
		}

		public static GlobalJsonExpressionReader Parse(string s) => new GlobalJsonExpressionReader(JObject.Parse(s));

		public GlobalJsonExpressionReader(JObject document)
			: base(document)
		{ }
	}
}
