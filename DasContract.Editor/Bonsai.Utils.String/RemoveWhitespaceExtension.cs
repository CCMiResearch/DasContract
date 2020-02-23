using System;
using System.Linq;

namespace Bonsai.Utils.String
{
    public static class RemoveWhitespaceExtension
    {
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}
