using System.Collections.Generic;

namespace Conductor.Abstractions
{
    public class Build : BuildInfo
    {
        public BuildChannel Channel { get; set; }
        public IEnumerable<Dependency> Assets { get; set; }
    }
}
