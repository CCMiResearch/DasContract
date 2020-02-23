using NUnit.Framework;
using Bonsai.Utils.String;

namespace Bonsai.Tests.String
{
    public class RemoveWhitespaceExtension
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RemoveWhiteSpaceSpaces()
        {
            Assert.AreEqual("1231231adc222", "123 123   1adc 222".RemoveWhitespace());
        }

        [Test]
        public void RemoveWhiteSpaceSpacesBreaklines()
        {
            Assert.AreEqual("1231231adc222", "123 123  \n 1adc\n 222".RemoveWhitespace());
        }
    }
}
