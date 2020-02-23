using NUnit.Framework;
using Bonsai.Utils.String;

namespace Bonsai.Tests.String
{
    public class ToIdFriendlyExtension
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AcceptableStart()
        {
            Assert.AreEqual("abcde123", "abcde123".ToIdFriendly());
            Assert.AreEqual("abcde123", "abc de 123".ToIdFriendly());
            Assert.AreEqual("abcde123", "ab!?c de 12|3##|".ToIdFriendly());
        }

        [Test]
        public void UnacceptableStart()
        {
            Assert.AreEqual("id-1abcde123", "1abc de 123".ToIdFriendly());
            Assert.AreEqual("id--abcde123", "-ab!?c de 12|3##|".ToIdFriendly());
            Assert.AreEqual("id-_abcde123", "_ab!?c de 12|3##|".ToIdFriendly());
        }

        [Test]
        public void Dash()
        {
            Assert.AreEqual("ab-cde-123", "ab-cde-123".ToIdFriendly());
            Assert.AreEqual("a-bcde123", "a-bc de 123".ToIdFriendly());
            Assert.AreEqual("a-bcde123", "a-b!?c de 12|3##|".ToIdFriendly());
        }

        [Test]
        public void Underscore()
        {
            Assert.AreEqual("ab_cde_123", "ab_cde_123".ToIdFriendly());
            Assert.AreEqual("a_bcde123", "a_bc de 123".ToIdFriendly());
            Assert.AreEqual("a_bcde123", "a_b!?c de 12|3##|".ToIdFriendly());
        }
    }
}
