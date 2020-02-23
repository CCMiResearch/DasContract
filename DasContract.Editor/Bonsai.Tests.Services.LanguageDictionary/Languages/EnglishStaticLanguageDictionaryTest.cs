using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Services.LanguageDictionary.Static.Languages;
using NUnit.Framework;

namespace Bonsai.Tests.Services.LanguageDictionary.Languages
{
    class EnglishStaticLanguageDictionaryTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CultureInfoGet()
        {
            var dictionary1 = new EnglishStaticLanguageDictionary(new Dictionary<string, string>());
            Assert.AreEqual("en", dictionary1.Culture.TwoLetterISOLanguageName);
        }
    }
}
