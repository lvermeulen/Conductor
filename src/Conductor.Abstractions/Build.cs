using System.Collections.Generic;

namespace Conductor.Abstractions
{
	public record Build(BuildChannel Channel, IList<Dependency> Assets);
}
