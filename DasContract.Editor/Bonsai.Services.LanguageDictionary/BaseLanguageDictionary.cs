using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Bonsai.Services.Interfaces;

namespace Bonsai.Services.LanguageDictionary
{
    public abstract class BaseLanguageDictionary : ILanguageDictionary
    {
        public const string DefaultContent = "";

        protected static bool IsDefaultContent(string sample)
        {
            return string.IsNullOrEmpty(sample);
        }

        public abstract CultureInfo Culture { get; set; }

        public abstract string GetContent(string id);
        public abstract Task<string> GetContentAsync(string id);

        public abstract bool TryGetContent(string id, out string content);
        public abstract ILanguageDictionaryResult TryGetContent(string id);
        public abstract Task<ILanguageDictionaryResult> TryGetContentAsync(string id);
    }
}
