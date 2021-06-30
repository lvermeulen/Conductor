using System.Collections.Generic;
using System.Text.Json;
using Conductor.Abstractions;

namespace Conductor.Channels.ExpressionReaders.SystemTextJson
{
	public abstract class SystemTextJsonExpressionReader : IExpressionReader
	{
		private readonly JsonDocument _document;

		protected SystemTextJsonExpressionReader(JsonDocument document)
		{
			_document = document;
		}

		public string ReadExpression(string expression)
		{
			var items = JsonSerializer.Deserialize<Dictionary<string, object>>(_document.RootElement.ToString() ?? "");
			if (items is null)
			{
				return null;
			}

			foreach ((string key, object value) in items)
			{
				if (key == expression)
				{
					return value?.ToString();
				}

				if (value is JsonElement { ValueKind: JsonValueKind.Object } jelem)
				{
					// ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
					foreach (var objItem in jelem.EnumerateObject())
					{
						if (objItem.NameEquals(expression))
						{
							return objItem.Value.ToString();
						}
					}
				}
			}

			return null;
		}
	}
}
