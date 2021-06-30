using Conductor.Abstractions;
using Newtonsoft.Json.Linq;

namespace Conductor.Channels.ExpressionReaders.NewtonsoftJson
{
	public abstract class NewtonsoftJsonExpressionReader : IExpressionReader
	{
		private readonly JObject _document;

		protected NewtonsoftJsonExpressionReader(JObject document)
		{
			_document = document;
		}

		public string ReadExpression(string expression)
		{
			foreach ((string key, var value) in _document)
			{
				if (key == expression)
				{
					return value?.Value<string>();
				}

				if (value is JObject jobj && jobj.ContainsKey(expression))
				{
					return jobj[expression]?.Value<string>();
				}
			}

			return null;
		}
	}
}
