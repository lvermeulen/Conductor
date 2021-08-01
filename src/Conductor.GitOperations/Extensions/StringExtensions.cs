using System.Text.RegularExpressions;

namespace Conductor.GitOperations.Extensions
{
	public static class StringExtensions
	{
		public static string ToKebabCase(this string s) => string.Join("-", new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+").Matches(s)).ToLower();
	}
}
