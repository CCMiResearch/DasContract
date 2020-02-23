using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Bonsai.Services.LanguageDictionary.Static.Languages
{
    public class CzechStaticLanguageDictionary : StaticLanguageDictionary
    {
        public CzechStaticLanguageDictionary(Dictionary<string, string> dictionary)
            : base(CultureInfo.GetCultureInfo("cs"), dictionary)
        {
            
        }
    }
}
