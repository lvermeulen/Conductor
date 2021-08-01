using System.Collections.Generic;

namespace Conductor.Abstractions
{
    public class DependencyMetadata
    {
        public IEnumerable<Dependency> Dependencies { get; }

        public DependencyMetadata(IEnumerable<Dependency> dependencies)
        {
            Dependencies = dependencies;
        }
    }
}
