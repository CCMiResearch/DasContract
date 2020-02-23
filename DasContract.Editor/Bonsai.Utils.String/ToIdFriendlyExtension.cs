using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bonsai.Utils.String
{
    public static class ToIdFriendlyExtension
    {
        public static string ToIdFriendly(this string input)
        {
            var acceptable = new Regex("[^a-zA-Z0-9_-]");
            var mustBeginWith = new Regex("[a-zA-Z]");

            input = acceptable.Replace(input, "");

            if (input.Length == 0)
                return "id-" + Guid.NewGuid().ToString();

            var firstLetter = input[0];
            var mustBeginWithMatch = mustBeginWith.Matches(firstLetter.ToString(CultureInfo.InvariantCulture));

            if (mustBeginWithMatch.Count == 0)
                input = "id-" + input;

            return input;
        }
    }
}
