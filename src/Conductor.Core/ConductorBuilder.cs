using System.Collections.Generic;
using Conductor.Abstractions;

namespace Conductor.Core
{
    public class ConductorBuilder
    {
        public IList<ExpressionDetailFile> ExpressionDetailFiles { get; } = new List<ExpressionDetailFile>();
        public IList<ExpressionFile> ExpressionFiles { get; } = new List<ExpressionFile>();

        public IConductorService Build() => new ConductorService(ExpressionDetailFiles, ExpressionFiles);
    }
}
