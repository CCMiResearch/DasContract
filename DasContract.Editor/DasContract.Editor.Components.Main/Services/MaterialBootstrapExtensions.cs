using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Bonsai.RazorComponents.MaterialBootstrap.Services.Scroll;
using Bonsai.Services.Interfaces;
using Blazor.FileReader;
using Bonsai.RazorComponents.MaterialBootstrap.Services.LanguageDictionary;
using Bonsai.RazorComponents.MaterialBootstrap.Services.LanguageDictionary.Languages;
using DasContract.Editor.Components.Main.Services.BusinessRuleActivityEditor;

namespace DasContract.Editor.Components.Main.Services
{
    public static class EditorMainComponentsExtensions
    {
        public static IServiceCollection AddEditorMainComponents(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddSingleton<ContractBusinessRuleActivityEditorService>();

            return servicesCollection;
        }
    }
}
