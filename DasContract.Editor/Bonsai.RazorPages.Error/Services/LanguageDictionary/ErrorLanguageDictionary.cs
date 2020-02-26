using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Services.LanguageDictionary.Static.Languages;

namespace Bonsai.RazorPages.Error.Services.LanguageDictionary
{
    public class ErrorLanguageDictionary
    {
        public const string Description = "Error - Description";
              
        public const string Keywords = "Error - Keywords";
               
        public const string GenericErrorHeader = "Error - Generic error header";
              
        public const string OfflineErrorHeader = "Error - Offline - Header";
             
        public static string ErrorHeader(string status) => $"Error - {status} - Header";
               
        public const string GenericErrorText = "Error - Generic error text";
               
        public static string ErrorText(string status) => $"Error - {status} - Text";
              
        public const string OfflineErrorText = "Error - Offline - Text";
              
        public const string BackButtonText = "Error - Back button";
    }
}
