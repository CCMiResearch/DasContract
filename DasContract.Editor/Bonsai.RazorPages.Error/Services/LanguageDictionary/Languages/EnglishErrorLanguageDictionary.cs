using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Services.LanguageDictionary.Static.Languages;

namespace Bonsai.RazorPages.Error.Services.LanguageDictionary.Languages
{
    public class EnglishErrorLanguageDictionary : EnglishStaticLanguageDictionary, IErrorLanguageDictionary
    {

        public EnglishErrorLanguageDictionary()
            :base(dictionary)
        {

        }

        public static readonly Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            { ErrorLanguageDictionary.Description, "Something went wrong with the server :(" },
            { ErrorLanguageDictionary.Keywords, "Error" },
            { ErrorLanguageDictionary.GenericErrorHeader, "Something went wrong" },
            { ErrorLanguageDictionary.GenericErrorText, "Something went wrong. We will try to fix it." },
            { ErrorLanguageDictionary.ErrorHeader("404"), "The page could not be found" },
            { ErrorLanguageDictionary.ErrorText("404"), "It does not exists or it has been deleted." },
            { ErrorLanguageDictionary.ErrorHeader("500"), "Server error" },
            { ErrorLanguageDictionary.ErrorText("500"), "Try to repeat your request. Alternatively, you can contact us." },
            { ErrorLanguageDictionary.OfflineErrorHeader, "Connection problem" },
            { ErrorLanguageDictionary.OfflineErrorText, "One of us has an internet problem." },
            { ErrorLanguageDictionary.BackButtonText, "Back to the homepage" },
        };
    }
}
