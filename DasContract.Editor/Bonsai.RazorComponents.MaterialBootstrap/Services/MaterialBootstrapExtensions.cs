using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Bonsai.RazorComponents.MaterialBootstrap.Services.Scroll;
using Bonsai.Services.Interfaces;
using Blazor.FileReader;
using Bonsai.RazorComponents.MaterialBootstrap.Services.LanguageDictionary;
using Bonsai.RazorComponents.MaterialBootstrap.Services.LanguageDictionary.Languages;

namespace Bonsai.RazorComponents.MaterialBootstrap.Services
{
    public static class MaterialBootstrapExtensions
    {
        public static IServiceCollection AddMaterialBootstrap(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddSingleton<IScroll, SlowScroll>();
            servicesCollection.AddSingleton<IMaterialBootstrapLanguageDictionary, EnglishMaterialBootstrapLanguageDictionary>();

            servicesCollection.AddFileReaderService(options => {
                options.UseWasmSharedBuffer = true;
            });

            return servicesCollection;
        }
    }
}
