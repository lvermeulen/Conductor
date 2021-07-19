using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conductor.Abstractions
{
    public class Repository
    {
        public IEnumerable<Dependency> Dependencies { get; set; }

        public Task CheckOutBranchForChanges()
        {
	        // TODO: implement
	        return Task.FromResult(true);
        }

        public Task UpdateAssets(IEnumerable<Dependency> dependencies)
        {
	        // TODO: implement
	        return Task.FromResult(true);
        }
    }
}
