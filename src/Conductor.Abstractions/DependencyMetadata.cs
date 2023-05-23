using System.Collections.Generic;
using System.Linq;

namespace Conductor.Abstractions
{
    public class DependencyMetadata
    {
        public IEnumerable<Dependency> ProductDependencies { get; }
        public IEnumerable<Dependency> TestDependencies { get; }
        public IEnumerable<Dependency> ToolSetDependencies { get; }

        public DependencyMetadata(IEnumerable<Dependency>? productDependencies = default, IEnumerable<Dependency>? testDependencies = default, IEnumerable<Dependency>? toolSetDependencies = default)
        {
            ProductDependencies = productDependencies ?? Enumerable.Empty<Dependency>();
            TestDependencies = testDependencies ?? Enumerable.Empty<Dependency>();
            ToolSetDependencies = toolSetDependencies ?? Enumerable.Empty<Dependency>();
        }
    }
}
