using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Services.Interfaces;

namespace Bonsai.Services.LanguageDictionary.Interfaces
{
    public interface IMergableLanguageDictionary<TDictionary> : ILanguageDictionary
    {
        /// <summary>
        /// Makes dictionary that includes this and the input dictionary words
        /// </summary>
        /// <param name="dictionaryToMerge">The other dictionary to merge with this one</param>
        /// <returns>New dictionary that contains this and the input dictionary words</returns>
        TDictionary Merge(ILanguageDictionary dictionaryToMerge);
    }
}
