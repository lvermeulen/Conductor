using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Conductor.Abstractions;
using Conductor.Abstractions.Extensions;

namespace Conductor.Channels.DependencyDetailsReaders.Xml
{
	public abstract class XmlDependencyDetailsReader : IDependencyDetailsReader
	{
		private readonly XDocument _document;

		protected XmlDependencyDetailsReader(XDocument document)
		{
			_document = document;
		}

		private static Dependency ReadDependency(XElement element, DependencyType dependencyType)
		{
			string name = element.Attribute("Name")?.Value;
			string version = element.Attribute("Version")?.Value;
			bool? pinned = (element.Attribute("Pinned")?.Value ?? "false").ToNullableBool();
			string url = element.Descendants("Uri").FirstOrDefault()?.Value;
			string sha = element.Descendants("Sha").FirstOrDefault()?.Value;
			string expression = element.Descendants("Expression").FirstOrDefault()?.Value;

			return new Dependency(name, version, sha, pinned, expression)
			{
				DependencyType = dependencyType,
				Url = url
			};
		}

		private IEnumerable<Dependency> ReadDependencies(string dependenciesNodeName, DependencyType dependencyType) => _document.Root
			?.Descendants(dependenciesNodeName)
			.FirstOrDefault()
			?.Descendants("Dependency")
			.Select(x => ReadDependency(x, dependencyType)) ?? Enumerable.Empty<Dependency>();

		private IEnumerable<Dependency> ReadDependencies()
		{
			var result = new List<Dependency>();
			result.AddRange(ReadDependencies("ProductDependencies", DependencyType.Product));
			result.AddRange(ReadDependencies("ToolsetDependencies", DependencyType.Toolset));
			result.AddRange(ReadDependencies("TestDependencies", DependencyType.Test));

			return result;
		}

		public Metadata ReadMetadata() => new Metadata(ReadDependencies());
	}
}
