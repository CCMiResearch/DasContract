using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Bonsai.Services.Interfaces;
using DasContract.Editor.Pages.Main.Services.FilePathProvider.SpecificFilePathProviders;
using DasContract.Editor.Pages.Main.Services.Entities;
using Bonsai.RazorComponents.MaterialBootstrap.Services;

namespace DasContract.Editor.Pages.Main
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //File path provider service
            //builder.Services.AddSingleton<IFilePathProvider, RegularFilePathProvider>();
            builder.Services.AddSingleton<IFilePathProvider, VersionedFilePathProvider>();

            //API services
            builder.Services.AddSingleton<ContractFileSessionService>();

            //Add material bootstrap
            builder.Services.AddMaterialBootstrap();

            await builder.Build().RunAsync();
        }
    }
}
