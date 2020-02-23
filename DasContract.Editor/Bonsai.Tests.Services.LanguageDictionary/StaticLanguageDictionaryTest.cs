using System.Collections.Generic;
using System.Globalization;
using Bonsai.Services.LanguageDictionary.Static;
using NUnit.Framework;

namespace Bonsai.Tests.Services.LanguageDictionary
{
    public class StaticLanguageDictionaryTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CultureInfoGet()
        {
            var dictionary1 = new StaticLanguageDictionary(CultureInfo.CreateSpecificCulture("cs"), new Dictionary<string, string>());
            Assert.AreEqual("cs", dictionary1.Culture.TwoLetterISOLanguageName);

            var dictionary2 = new StaticLanguageDictionary(CultureInfo.GetCultureInfo("cs"), new Dictionary<string, string>());
            Assert.AreEqual("cs", dictionary2.Culture.TwoLetterISOLanguageName);
        }

        [Test]
        public void GetContent()
        {
            var dictionary = new StaticLanguageDictionary(CultureInfo.CreateSpecificCulture("cs"), new Dictionary<string, string>() {
                { "1", "one" },
                { "2", "two" },
                { "3", "three" },
            });

            Assert.AreEqual("one", dictionary.GetContent("1"));
            Assert.AreEqual("two", dictionary.GetContent("2"));
            Assert.AreEqual("three", dictionary.GetContent("3"));
        }

        [Test]
        public void GetContentDefault()
        {
            var dictionary = new StaticLanguageDictionary(CultureInfo.CreateSpecificCulture("cs"), new Dictionary<string, string>() {
                { "1", "one" },
                { "2", "two" },
                { "3", "three" },
            });

            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, dictionary.GetContent("xx"));
            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, dictionary.GetContent(""));
            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, dictionary.GetContent("yxz"));
        }

        [Test]
        public void TryGetContent()
        {
            var dictionary = new StaticLanguageDictionary(CultureInfo.CreateSpecificCulture("cs"), new Dictionary<string, string>() {
                { "1", "one" },
                { "2", "two" },
                { "3", "three" },
            });


            Assert.IsTrue(dictionary.TryGetContent("1", out var content1));
            Assert.IsTrue(dictionary.TryGetContent("2", out var content2));
            Assert.IsTrue(dictionary.TryGetContent("3", out var content3));

            Assert.AreEqual("one", content1);
            Assert.AreEqual("two", content2);
            Assert.AreEqual("three", content3);
        }

        [Test]
        public void TryGetContentDefault()
        {
            var dictionary = new StaticLanguageDictionary(CultureInfo.CreateSpecificCulture("cs"), new Dictionary<string, string>() {
                { "1", "one" },
                { "2", "two" },
                { "3", "three" },
            });


            Assert.IsFalse(dictionary.TryGetContent("xx", out var content1));
            Assert.IsFalse(dictionary.TryGetContent("", out var content2));
            Assert.IsFalse(dictionary.TryGetContent("xyz", out var content3));

            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, content1);
            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, content2);
            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, content3);
        }

    }
}
