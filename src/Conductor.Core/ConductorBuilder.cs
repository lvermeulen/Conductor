using System.Collections.Generic;
using Conductor.Abstractions;

namespace Conductor.Core
{
    public class ConductorBuilder
    {
	    public IList<DependencyDetailsFile> DependencyDetailsFiles { get; } = new List<DependencyDetailsFile>();
        public IList<ExpressionFile> ExpressionFiles { get; } = new List<ExpressionFile>();
        public JsonSerializerType JsonSerializer { get; set; } = JsonSerializerType.SystemTextJson;

        public IConductorService Build() => new ConductorService(DependencyDetailsFiles, ExpressionFiles, JsonSerializer);
    }
}
