using System.IO;
using System.Xml.Linq;

namespace Conductor.Channels.DependencyDetailsReaders.Xml
{
	public class VersionDetailsXmlReader : XmlDependencyDetailsReader
	{
		public static VersionDetailsXmlReader LoadFrom(string fileName)
		{
			using var stm = new FileStream(fileName, FileMode.Open);
			return new VersionDetailsXmlReader(XDocument.Load(stm, LoadOptions.None));
		}

		public static VersionDetailsXmlReader Parse(string s) => new VersionDetailsXmlReader(XDocument.Parse(s));

		public VersionDetailsXmlReader(XDocument document)
			: base(document)
		{ }
	}
}
