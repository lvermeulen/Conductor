using GitOpsRepositories.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace GitOpsRepositories.Tests
{
	public class StringExtensionsShould
	{
		private readonly ITestOutputHelper _testOutputHelper;

		public StringExtensionsShould(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Theory]
		[InlineData("MyStringWithCAPITALLetters", "my-string-with-capital-letters")]
		[InlineData("My String With Spaces", "my-string-with-spaces")]
		[InlineData("My String With Number 3000 and that's it", "my-string-with-number-3000-and-that-s-it")]
		public void ConvertToKebabCase(string s, string expected)
		{
			string original = s;
			s = s.ToKebabCase();
			bool equals = s == expected;
			_testOutputHelper.WriteLine(equals
				? $"{original} => {s} == {expected}"
				: $"{original} => {s} != {expected}");

			Assert.True(equals);
		}
	}
}
