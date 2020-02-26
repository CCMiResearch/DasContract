using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bonsai.Services.Interfaces;
using Bonsai.RazorPages.Error.Services.LanguageDictionary;

namespace Bonsai.RazorPages.Error.Areas.ErrorPages.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorPageModel : ErrorLayoutModel
    {
        //Example: https://github.com/aspnet/AspNetCore.Docs/blob/master/aspnetcore/fundamentals/error-handling/samples/2.x/ErrorHandlingSample/Pages/StatusCode.cshtml.cs

        public ErrorPageModel(IErrorLanguageDictionary languageDictionary, IFilePathProvider pathProvider)
        {
            LanguageDictionary = languageDictionary;
            PathProvider = pathProvider;
        }

        /// <summary>
        /// Error status code of this error
        /// </summary>
        public string ErrorStatusCode { get; set; }

        /// <summary>
        /// The original reguest URL
        /// </summary>
        public string OriginalURL { get; set; }

        /// <summary>
        /// Returns true if the original URL was caught
        /// </summary>
        public bool HasOriginalURL => !string.IsNullOrEmpty(OriginalURL);

        public override string Title => "Error " + ErrorStatusCode;

        public override string Description => LanguageDictionary.GetContent(ErrorLanguageDictionary.Description);

        public override string Keywords => LanguageDictionary.GetContent(ErrorLanguageDictionary.Keywords);

        /// <summary>
        /// Returns paths to the og image path
        /// </summary>
        public override string OgImagePath
        {
            get
            {
                if (ErrorStatusCode == "404")
                    return PathProvider.PathTo("/Images/OG/og_404.png", LibraryInfo.Name);
                else if (ErrorStatusCode == "500")
                    return PathProvider.PathTo("/Images/OG/og_500.png", LibraryInfo.Name);
                return PathProvider.PathTo("/Images/OG/og_500.png", LibraryInfo.Name); ;
            }
        }

        /// <summary>
        /// Returns path to the error icon path
        /// </summary>
        public string IconPath 
        {
            get
            {
                if (ErrorStatusCode == "404")
                    return PathProvider.PathTo("/Images/Icons/magnifier.svg", LibraryInfo.Name);
                else if (ErrorStatusCode == "500")
                    return PathProvider.PathTo("/Images/Icons/server.svg", LibraryInfo.Name);
                return PathProvider.PathTo("/Images/Icons/server.svg", LibraryInfo.Name);
            }
        }

        /// <summary>
        /// Returns text for the error description
        /// </summary>
        public string Text { 
            get
            {
                return LanguageDictionary.TryGetContent(ErrorLanguageDictionary.ErrorText(ErrorStatusCode), out string text)
                    ? text
                    : LanguageDictionary.GetContent(ErrorLanguageDictionary.GenericErrorText);
            } 
        }


        /// <summary>
        /// Returns header text of the page
        /// </summary>
        public string Header 
        { 
            get
            {
                return LanguageDictionary.TryGetContent(ErrorLanguageDictionary.ErrorHeader(ErrorStatusCode), out string header)
                    ? header
                    : LanguageDictionary.GetContent(ErrorLanguageDictionary.GenericErrorHeader);
            }
        }

        public void OnGet(string code)
        {
            ErrorStatusCode = code;

            IStatusCodeReExecuteFeature statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCodeReExecuteFeature != null)
            {
                OriginalURL =
                    statusCodeReExecuteFeature.OriginalPathBase
                    + statusCodeReExecuteFeature.OriginalPath
                    + statusCodeReExecuteFeature.OriginalQueryString;
            }
        }
    }
}
