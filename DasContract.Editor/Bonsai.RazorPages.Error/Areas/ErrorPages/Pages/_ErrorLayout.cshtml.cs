using Microsoft.AspNetCore.Mvc.RazorPages;
using Bonsai.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.RazorPages.Error.Services.LanguageDictionary;

namespace Bonsai.RazorPages.Error.Areas.ErrorPages.Pages
{
    public abstract class ErrorLayoutModel : PageModel
    {

        public IFilePathProvider PathProvider { get; protected set; }

        public IErrorLanguageDictionary LanguageDictionary { get; protected set; }

        public abstract string Title { get; }

        public abstract string Description { get; }

        public abstract string Keywords { get; }

        public abstract string OgImagePath { get; }

        /// <summary>
        /// Returns text of the "go back" button
        /// </summary>
        public string BackButtonText => LanguageDictionary.GetContent(ErrorLanguageDictionary.BackButtonText);
    }
}
