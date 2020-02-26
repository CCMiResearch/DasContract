using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Services.LanguageDictionary.Static.Languages;

namespace Bonsai.RazorPages.Error.Services.LanguageDictionary.Languages
{
    public class CzechErrorLanguageDictionary: CzechStaticLanguageDictionary, IErrorLanguageDictionary
    {

        public CzechErrorLanguageDictionary()
            :base(dictionary)
        {

        }

        public static readonly Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { ErrorLanguageDictionary.Description, "Na stránce se něco pokazilo :(" },
            { ErrorLanguageDictionary.Keywords, "Error" },
            { ErrorLanguageDictionary.GenericErrorHeader, "Něco se pokazilo" },
            { ErrorLanguageDictionary.GenericErrorText, "Se stránkou je něco špatně. Pokusíme se to opravit." },
            { ErrorLanguageDictionary.ErrorHeader("404"), "Stránku se bohužel nedaří nalézt" },
            { ErrorLanguageDictionary.ErrorText("404"), "Buď neexistuje, nebo byla odstraněna." },
            { ErrorLanguageDictionary.ErrorHeader("500"), "Chyba na straně serveru" },
            { ErrorLanguageDictionary.ErrorText("500"), "Zkuste váš požadavek provést znovu. Případně nás prosím kontaktujte." },
            { ErrorLanguageDictionary.OfflineErrorHeader, "Problém s připojením" },
            { ErrorLanguageDictionary.OfflineErrorText, "Někdo z nás nemá přístup k internetu." },
            { ErrorLanguageDictionary.BackButtonText, "Zpět na domovskou stránku" },
        };
    }
}
