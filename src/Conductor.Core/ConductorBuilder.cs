using System.Collections.Generic;

namespace Conductor.Core
{
	public class ConductorBuilder
	{
		public IList<ExpressionDetailFile> ExpressionDetailFiles { get; set; } = new List<ExpressionDetailFile>();
		public IList<ExpressionFile> ExpressionFiles { get; set; } = new List<ExpressionFile>();

		public ConductorService Build() => new ConductorService(ExpressionDetailFiles, ExpressionFiles);
	}
}
