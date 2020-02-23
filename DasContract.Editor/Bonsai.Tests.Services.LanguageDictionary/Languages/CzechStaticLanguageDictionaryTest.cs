using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Services.LanguageDictionary.Static.Languages;
using NUnit.Framework;

namespace Bonsai.Tests.Services.LanguageDictionary.Languages
{
    class CzechStaticLanguageDictionaryTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CultureInfoGet()
        {
            var dictionary1 = new CzechStaticLanguageDictionary(new Dictionary<string, string>());
            Assert.AreEqual("cs", dictionary1.Culture.TwoLetterISOLanguageName);
        }
    }
}
