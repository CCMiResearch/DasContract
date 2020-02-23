using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Services.Interfaces;

namespace Bonsai.Services.LanguageDictionary.Result
{
    public class LanguageDictionaryResult : ILanguageDictionaryResult
    {
        public LanguageDictionaryResult(bool success, string content)
        {
            Success = success;
            Content = content;
        }

        public bool Success { get; set; }

        public string Content { get; set; }


        /// <summary>
        /// Returns successful result
        /// </summary>
        /// <param name="content">The retrieved content</param>
        /// <returns>Sucessful language dictionary search result</returns>
        public static LanguageDictionaryResult Found(string content)
        {
            return new LanguageDictionaryResult(true, content);
        }

        /// <summary>
        /// Returns failed result
        /// </summary>
        /// <returns>Failed language dictionary search result</returns>
        public static LanguageDictionaryResult NotFound()
        {
            return notFoundInstance;
        }
        static readonly LanguageDictionaryResult notFoundInstance = new LanguageDictionaryResult(false, "");
    }
}
