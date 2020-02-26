using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bonsai.Services.Interfaces;
using Bonsai.RazorPages.Error.Services.LanguageDictionary;

namespace Bonsai.RazorPages.Error.Areas.ErrorPages.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class OfflinePageModel : ErrorLayoutModel
    {

        public OfflinePageModel(IErrorLanguageDictionary languageDictionary, IFilePathProvider pathProvider)
        {
            LanguageDictionary = languageDictionary;
            PathProvider = pathProvider;
        }

        /// <summary>
        /// Returns offline icon
        /// </summary>
        public string IconPath => PathProvider.CleanPathTo("/Images/Icons/offline.svg", LibraryInfo.Name);

        /// <summary>
        /// Returns offline error text
        /// </summary>
        public string Text => LanguageDictionary.GetContent(ErrorLanguageDictionary.OfflineErrorText);

        /// <summary>
        /// Returns offline error header
        /// </summary>
        public string Header => LanguageDictionary.GetContent(ErrorLanguageDictionary.OfflineErrorHeader);

        public override string Title => "Offline";

        public override string Description => Text;

        public override string Keywords => "Offline";

        public override string OgImagePath => PathProvider.CleanPathTo("/Images/OG/og_offline.png", LibraryInfo.Name);

        public void OnGet()
        {
        }
    }
}
