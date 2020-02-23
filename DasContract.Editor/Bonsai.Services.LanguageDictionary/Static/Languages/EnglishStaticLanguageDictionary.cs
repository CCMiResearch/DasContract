using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Bonsai.Services.LanguageDictionary.Static.Languages
{
    public class EnglishStaticLanguageDictionary : StaticLanguageDictionary
    {
        public EnglishStaticLanguageDictionary(Dictionary<string, string> dictionary)
            : base(CultureInfo.GetCultureInfo("en"), dictionary)
        {

        }
    }
}
