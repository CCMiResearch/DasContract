using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Bonsai.Services.Interfaces
{
    public interface ILanguageDictionary
    {
        /// <summary>
        /// Returns a content based on the input id and currently set language
        /// </summary>
        /// <param name="id">The content id</param>
        /// <returns>Content with matching input id</returns>
        string GetContent(string id);

        /// <summary>
        /// Returns a content based on the input id and currently set language
        /// </summary>
        /// <param name="id">The content id</param>
        /// <returns>Task with content with matching input id</returns>
        Task<string> GetContentAsync(string id);

        /// <summary>
        /// Tries to get content and if it exists, it is return in the content output parameter.
        /// </summary>
        /// <param name="id">The content id</param>
        /// <param name="content">The content if exists</param>
        /// <returns>True if the content exists, else false</returns>
        bool TryGetContent(string id, out string content);

        /// <summary>
        /// Tries to get content and if it exists, it is return in the language dictionary result.
        /// </summary>
        /// <param name="id">The content id</param>
        /// <returns>Language dictionary result that tells if the search was success and the content</returns>
        ILanguageDictionaryResult TryGetContent(string id);

        /// <summary>
        /// Tries to get content and if it exists, it is return in the language dictionary result.
        /// </summary>
        /// <param name="id">The content id</param>
        /// <returns>Task with language dictionary result that tells if the search was success and the content</returns>
        Task<ILanguageDictionaryResult> TryGetContentAsync(string id);

        /// <summary>
        /// Contains current culture info
        /// </summary>
        CultureInfo Culture { get; set; }
    }

    public interface ILanguageDictionaryResult
    {
        /// <summary>
        /// Tells if there is some content
        /// </summary>
        bool Success { get; set; }

        /// <summary>
        /// The content (if there is some)
        /// </summary>
        string Content { get; set; }
    }
}


