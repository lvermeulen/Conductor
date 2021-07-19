using System;

namespace Conductor.Abstractions.Extensions
{
    public static class StringExtensions
    {
        public static bool? ToNullableBool(this string s) => s.Equals("true", StringComparison.InvariantCultureIgnoreCase);
    }
}
