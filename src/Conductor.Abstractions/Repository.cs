using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Conductor.Abstractions
{
	public class Repository
    {
	    private static readonly GitOperations.GitOperations s_gitOps = new GitOperations.GitOperations();

        private readonly string _repositoryPath;

        public IList<Dependency> Dependencies { get; set; } = new List<Dependency>();

        public Repository(string repositoryPath)
        {
	        _repositoryPath = repositoryPath;
        }

        public Task CheckOutBranchForChangesAsync(string branchName, CancellationToken cancellationToken = default) => s_gitOps.CheckoutRepositoryBranchAsync(_repositoryPath, branchName, cancellationToken);

        public async Task MergeChangesAsync(string targetBranchName, CancellationToken cancellationToken = default)
        {
	        if (cancellationToken.IsCancellationRequested)
	        {
		        return;
	        }

	        await s_gitOps.MergeChangesAsync(_repositoryPath, targetBranchName, cancellationToken);
        }

        private (IEnumerable<Dependency> Inserts, IEnumerable<Dependency> Updates, IEnumerable<Dependency> Deletes) FindDifferences(IEnumerable<Dependency> newDependencies)
        {
	        var newDependenciesList = newDependencies.ToList();
	        var inserts = Dependencies.Except(newDependenciesList);
	        var updates = Dependencies.Join(newDependenciesList, dependency => dependency.Name, newDependency => newDependency.Name, (dependency, newDependency) => dependency);
	        var deletes = newDependenciesList.Except(Dependencies);

	        return (inserts, updates, deletes);
        }

        public Task UpdateAssetsAsync(IEnumerable<Dependency> dependencies, CancellationToken cancellationToken)
        {
	        if (cancellationToken.IsCancellationRequested)
	        {
		        return Task.FromResult(false);
	        }

            // find inserts, updates, deletes
            var (inserts, updates, deletes) = FindDifferences(dependencies);

            foreach (var insert in inserts)
            {
	            Dependencies.Add(insert);
            }

            foreach (var update in updates)
            {
	            var dependency = Dependencies.FirstOrDefault(x => x.Name.Equals(update.Name, StringComparison.InvariantCultureIgnoreCase));
	            if (dependency?.Pinned == true)
	            {
		            continue;
	            }

	            if (dependency != null)
	            {
		            int index = Dependencies.IndexOf(dependency);
		            if (index >= 0)
		            {
			            Dependencies.RemoveAt(index);
			            Dependencies.Add(update);
		            }
	            }
            }

            foreach (var delete in deletes)
            {
	            Dependencies.Remove(delete);
            }

            return Task.FromResult(true);
        }
    }
}
