using System.Collections.Generic;
using System.Globalization;
using Bonsai.Services.Interfaces;
using Bonsai.Services.LanguageDictionary.Combined;
using Bonsai.Services.LanguageDictionary.Static;
using Bonsai.Services.LanguageDictionary.Static.Languages;
using NUnit.Framework;

namespace Bonsai.Tests.Services.LanguageDictionary
{
    public class CombinedLanguageDictionaryTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CultureGet()
        {
            var dictionary1 = new EnglishStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "1", "one" }
            });

            var dictionary2 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "2", "two" }
            });

            var dictionary3 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "3", "three" }
            });

            var mergedDictionary = new CombinedLanguageDictionary(new List<ILanguageDictionary>() { dictionary1, dictionary2, dictionary3 });

            Assert.AreEqual("en", mergedDictionary.Culture.TwoLetterISOLanguageName);

            mergedDictionary.Culture = CultureInfo.GetCultureInfo("cs");
            Assert.AreEqual("cs", mergedDictionary.Culture.TwoLetterISOLanguageName);
        }

        [Test]
        public void GetContent()
        {
            var dictionary1 = new EnglishStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "1", "one" }
            });

            var dictionary2 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "2", "two" }
            });

            var dictionary3 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "3", "three" }
            });

            var mergedDictionary = new CombinedLanguageDictionary(new List<ILanguageDictionary>() { dictionary1, dictionary2, dictionary3 });

            Assert.AreEqual("one", mergedDictionary.GetContent("1"));
            Assert.AreEqual("two", mergedDictionary.GetContent("2"));
            Assert.AreEqual("three", mergedDictionary.GetContent("3"));
        }

        [Test]
        public void GetContentDefault()
        {
            var dictionary1 = new EnglishStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "1", "one" }
            });

            var dictionary2 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "2", "two" }
            });

            var dictionary3 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "3", "three" }
            });

            var mergedDictionary = new CombinedLanguageDictionary(new List<ILanguageDictionary>() { dictionary1, dictionary2, dictionary3 });

            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, mergedDictionary.GetContent("xx"));
            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, mergedDictionary.GetContent(""));
            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, mergedDictionary.GetContent("xyz"));
        }

        [Test]
        public void TryGetContent()
        {
            var dictionary1 = new EnglishStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "1", "one" }
            });

            var dictionary2 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "2", "two" }
            });

            var dictionary3 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "3", "three" }
            });

            var mergedDictionary = new CombinedLanguageDictionary(new List<ILanguageDictionary>() { dictionary1, dictionary2, dictionary3 });

            Assert.IsTrue(mergedDictionary.TryGetContent("1", out var content1));
            Assert.IsTrue(mergedDictionary.TryGetContent("2", out var content2));
            Assert.IsTrue(mergedDictionary.TryGetContent("3", out var content3));

            Assert.AreEqual("one", content1);
            Assert.AreEqual("two", content2);
            Assert.AreEqual("three", content3);
        }

        [Test]
        public void TryGetContentDefault()
        {
            var dictionary1 = new EnglishStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "1", "one" }
            });

            var dictionary2 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "2", "two" }
            });

            var dictionary3 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "3", "three" }
            });

            var mergedDictionary = new CombinedLanguageDictionary(new List<ILanguageDictionary>() { dictionary1, dictionary2, dictionary3 });

            Assert.IsFalse(mergedDictionary.TryGetContent("xx", out var content1));
            Assert.IsFalse(mergedDictionary.TryGetContent("", out var content2));
            Assert.IsFalse(mergedDictionary.TryGetContent("xyz", out var content3));

            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, content1);
            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, content2);
            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, content3);
        }

        [Test]
        public void Merge()
        {
            var dictionary1 = new EnglishStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "1", "one" }
            });

            var dictionary2 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "2", "two" }
            });

            var dictionary3 = new CzechStaticLanguageDictionary(new Dictionary<string, string>()
            {
                { "3", "three" }
            });

            var mergedDictionary = CombinedLanguageDictionary.Merge(dictionary1, dictionary2, dictionary3);

            Assert.IsTrue(mergedDictionary.TryGetContent("1", out var content1));
            Assert.IsTrue(mergedDictionary.TryGetContent("2", out var content2));
            Assert.IsTrue(mergedDictionary.TryGetContent("3", out var content3));
            Assert.IsFalse(mergedDictionary.TryGetContent("", out var content4));

            Assert.AreEqual("one", content1);
            Assert.AreEqual("two", content2);
            Assert.AreEqual("three", content3);
            Assert.AreEqual(Bonsai.Services.LanguageDictionary.BaseLanguageDictionary.DefaultContent, content4);
        }
    }
}
