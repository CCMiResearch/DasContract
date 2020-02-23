using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bonsai.Services.Interfaces;
using Bonsai.Services.LanguageDictionary.Result;

namespace Bonsai.Services.LanguageDictionary.Static
{
    public class StaticLanguageDictionary: BaseLanguageDictionary
    {
        public Dictionary<string, string> Dictionary { get; set; }

        public override CultureInfo Culture { get; set; }

        public StaticLanguageDictionary(CultureInfo cultureInfo, Dictionary<string, string> dictionary)
        {
            Culture = cultureInfo;
            Dictionary = dictionary;
        }

        public override string GetContent(string id) => Dictionary.TryGetValue(id, out string content) ? content : DefaultContent;

        public override bool TryGetContent(string id, out string content)
        {
            if (Dictionary.TryGetValue(id, out string contentRes))
            {
                content = contentRes;
                return true;
            }

            content = DefaultContent;
            return false;
        }

        public static StaticLanguageDictionary Empty()
        {
            return new StaticLanguageDictionary(
                CultureInfo.DefaultThreadCurrentCulture, 
                new Dictionary<string, string>()
                );
        }

        public override Task<string> GetContentAsync(string id)
        {
            return Task.FromResult(GetContent(id));
        }

        public Task<bool> TryGetContentAsync(string id, out string content)
        {
            return Task.FromResult(TryGetContent(id, out content));
        }

        public override ILanguageDictionaryResult TryGetContent(string id)
        {
            if (TryGetContent(id, out var content))
                return LanguageDictionaryResult.Found(content);
            return LanguageDictionaryResult.NotFound();
        }

        public override Task<ILanguageDictionaryResult> TryGetContentAsync(string id)
        {
            if (TryGetContent(id, out var content))
                return Task.FromResult(LanguageDictionaryResult.Found(content) as ILanguageDictionaryResult);
            return Task.FromResult(LanguageDictionaryResult.NotFound() as ILanguageDictionaryResult);
        }
    }
}
