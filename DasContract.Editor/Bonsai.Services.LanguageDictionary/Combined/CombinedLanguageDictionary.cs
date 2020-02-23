using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bonsai.Services.Interfaces;
using Bonsai.Services.LanguageDictionary.Interfaces;
using Bonsai.Services.LanguageDictionary.Result;
using Bonsai.Services.LanguageDictionary.Static;

namespace Bonsai.Services.LanguageDictionary.Combined
{
    public class CombinedLanguageDictionary: BaseLanguageDictionary, IMergableLanguageDictionary<CombinedLanguageDictionary>
    {
        readonly List<ILanguageDictionary> dictionaries;

        public CombinedLanguageDictionary(IEnumerable<ILanguageDictionary> dictionaries)
        {
            if (!dictionaries.Any())
                throw new ArgumentException("CombinedDictionary requires at least one initial dictionary");

            this.dictionaries = dictionaries.ToList();
        }

        public override CultureInfo Culture
        {
            get
            {
                if (culture == null)
                    return dictionaries.First().Culture;
                return culture;
            }
            set
            {
                culture = value;
            }
        }

        CultureInfo culture = null;


        public override string GetContent(string id)
        {
            foreach(var dictionary in dictionaries)
            {
                var res = dictionary.GetContent(id);
                if (!IsDefaultContent(res))
                    return res;
            }

            return DefaultContent;
        }
        public override async Task<string> GetContentAsync(string id)
        {
            foreach (var dictionary in dictionaries)
            {
                var res = await dictionary.GetContentAsync(id);
                if (!IsDefaultContent(res))
                    return res;
            }

            return DefaultContent;
        }

        public override bool TryGetContent(string id, out string content)
        {
            foreach (var dictionary in dictionaries)
            {
                if (dictionary.TryGetContent(id, out var res))
                {
                    content = res;
                    return true;
                }
            }

            content = DefaultContent;
            return false;
        }

        public override ILanguageDictionaryResult TryGetContent(string id)
        {
            if (TryGetContent(id, out var content))
                return LanguageDictionaryResult.Found(content);
            return LanguageDictionaryResult.NotFound();
        }

        public override async Task<ILanguageDictionaryResult> TryGetContentAsync(string id)
        {
            foreach (var dictionary in dictionaries)
            {
                var dRes = await dictionary.TryGetContentAsync(id);
                if (dRes.Success)
                    return dRes;
            }

            return LanguageDictionaryResult.NotFound();
        }

        public CombinedLanguageDictionary Merge(ILanguageDictionary dictionaryToMerge)
        {
            var newList = dictionaries.Concat(new List<ILanguageDictionary>() { dictionaryToMerge });
            return new CombinedLanguageDictionary(newList);
        }

        /// <summary>
        /// Merges dictionaries into one combined
        /// </summary>
        /// <param name="dictionaries">Dictionaries to merge</param>
        /// <returns>New merged dictionary</returns>
        public static CombinedLanguageDictionary Merge(params ILanguageDictionary[] dictionaries)
        {
            if (dictionaries.Length == 0)
                return Empty();

            var resultDictionary = new CombinedLanguageDictionary(dictionaries.ToList());

            return resultDictionary;
        }

        /// <summary>
        /// Returns empty dictionary
        /// </summary>
        /// <returns>Empty dictionary</returns>
        public static CombinedLanguageDictionary Empty()
        {
            return new CombinedLanguageDictionary(new List<ILanguageDictionary>() { StaticLanguageDictionary.Empty() });
        }
    }
}
