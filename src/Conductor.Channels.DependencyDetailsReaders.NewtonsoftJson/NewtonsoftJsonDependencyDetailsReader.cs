using System;
using Conductor.Abstractions;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Conductor.Channels.DependencyDetailsReaders.NewtonsoftJson
{
    public abstract class NewtonsoftJsonDependencyDetailsReader : IDependencyDetailsReader
    {
        private readonly JObject _document;

        protected NewtonsoftJsonDependencyDetailsReader(JObject document)
        {
            _document = document;
        }

        private IEnumerable<Dependency> ReadDependencies(string dependenciesNodeName, DependencyType dependencyType)
        {
            foreach ((string key, var value) in _document)
            {
                if (key.Equals("Dependencies", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach ((string valueItemKey, var valueItemValue) in (JObject)value)
                    {
                        if (valueItemKey.Equals(dependenciesNodeName, StringComparison.InvariantCultureIgnoreCase) && valueItemValue is not null)
                        {
                            var dependencies = valueItemValue
                                .ToObject<IEnumerable<Dependency>>();

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

        public Metadata ReadMetadata() => new Metadata(ReadDependencies());
    }
}
