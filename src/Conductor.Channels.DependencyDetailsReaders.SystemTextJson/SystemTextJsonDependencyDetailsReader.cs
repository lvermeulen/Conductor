using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Conductor.Abstractions;

namespace Conductor.Channels.DependencyDetailsReaders.SystemTextJson
{
    public abstract class SystemTextJsonDependencyDetailsReader : IDependencyDetailsReader
    {
        private readonly JsonDocument _document;

        protected SystemTextJsonDependencyDetailsReader(JsonDocument document)
        {
            _document = document;
        }

        private IEnumerable<Dependency> ReadDependencies(string dependenciesNodeName, DependencyType dependencyType)
        {
            var items = JsonSerializer.Deserialize<Dictionary<string, object>>(_document.RootElement.ToString());
            if (items is null)
            {
                return Enumerable.Empty<Dependency>();
            }

            foreach (var (key, value) in items)
            {
                if (key.Equals("Dependencies", StringComparison.InvariantCultureIgnoreCase))
                {
                    // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                    foreach (var valueItem in ((JsonElement)value).EnumerateObject())
                    {
                        if (valueItem.Name.Equals(dependenciesNodeName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var json = valueItem.Value.ToString();
                            var dependencies = JsonSerializer.Deserialize<IEnumerable<Dependency>>(json);

                            foreach (var dependency in dependencies ?? Enumerable.Empty<Dependency>())
                            {
                                dependency.DependencyType = dependencyType;
                            }

                            return dependencies;
                        }
                    }
                }
            }

            return Enumerable.Empty<Dependency>();
        }

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
