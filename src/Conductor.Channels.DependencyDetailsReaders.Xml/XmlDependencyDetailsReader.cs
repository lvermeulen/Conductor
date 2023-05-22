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
            var name = element.Attribute("Name")?.Value;
            var version = element.Attribute("Version")?.Value;
            var pinned = (element.Attribute("Pinned")?.Value ?? "false").ToNullableBool();
            var sha = element.Descendants("Sha").FirstOrDefault()?.Value;
            var expression = element.Descendants("Expression").FirstOrDefault()?.Value;

            return new Dependency(name, version, sha, pinned, expression)
            {
                DependencyType = dependencyType
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

        public DependencyMetadata ReadMetadata() => new DependencyMetadata(ReadDependencies());
    }
}
