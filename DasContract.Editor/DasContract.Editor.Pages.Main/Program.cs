using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Bonsai.Services.Interfaces;
using DasContract.Editor.Pages.Main.Services.FilePathProvider.SpecificFilePathProviders;

namespace DasContract.Editor.Pages.Main
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //builder.Services.AddSingleton<IFilePathProvider, RegularFilePathProvider>();
            builder.Services.AddSingleton<IFilePathProvider, VersionedFilePathProvider>();

            await builder.Build().RunAsync();
        }
    }
}
