using System.Linq;
using System.Xml.Linq;
using Conductor.Abstractions;

namespace Conductor.Channels.ExpressionReaders.Xml
{
	public abstract class XmlExpressionReader : IExpressionReader
	{
		private readonly XDocument _document;

		protected XmlExpressionReader(XDocument document)
		{
			_document = document;
		}

		public string ReadExpression(string expression) => _document.Root
			?.Descendants(expression)
			.FirstOrDefault()
			?.Value;
	}
}
