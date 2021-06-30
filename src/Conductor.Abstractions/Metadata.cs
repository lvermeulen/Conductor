using System.Collections.Generic;

namespace Conductor.Abstractions
{
	public class Metadata
	{
		public IEnumerable<Dependency> Dependencies { get; }

		public Metadata(IEnumerable<Dependency> dependencies)
		{
			Dependencies = dependencies;
		}
	}
}
