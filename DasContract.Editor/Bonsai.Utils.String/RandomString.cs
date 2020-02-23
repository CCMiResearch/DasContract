using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bonsai.Utils.String
{
    public static class RandomString
    {
        private static Random RandomSys { get; set; } = new Random();

        private static string Chars { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string Random(int length)
        {
            return new string(Enumerable.Repeat(Chars, length)
              .Select(s => s[RandomSys.Next(s.Length)]).ToArray());
        }

        public static string Random(this string input, int length)
        {
            return new string(Random(length).ToCharArray());
        }
    }
}
