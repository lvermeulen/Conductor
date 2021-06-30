using System.IO;
using Conductor.Abstractions;

namespace Conductor.Channels.ExpressionReaders.Text
{
	public class TextExpressionReader : IExpressionReader
	{
		public string ReadExpression(string expression)
		{
			if (!Path.HasExtension(expression))
			{
				expression += ".txt";
			}

			return File.Exists(expression)
				? File.ReadAllText(expression)
				: null;
		}
	}
}
