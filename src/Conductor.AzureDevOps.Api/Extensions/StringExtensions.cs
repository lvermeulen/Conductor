using System;
using Conductor.GitOperations.Extensions;

namespace Conductor.AzureDevOps.Api.Extensions
{
	public static class StringExtensions
	{
		public static string MakeRefSpec(this string s) => s.StartsWith("refs/heads/", StringComparison.Ordinal)
			? s.ToKebabCase()
			: $"refs/heads/{s.ToKebabCase()}";
	}
}
