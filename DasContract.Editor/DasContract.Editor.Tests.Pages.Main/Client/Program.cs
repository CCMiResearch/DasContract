using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Bonsai.RazorComponents.MaterialBootstrap.Services;
using DasContract.Editor.Components.Main.Services;

namespace DasContract.Editor.Tests.Components.Main.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddMaterialBootstrap();

            builder.Services.AddEditorMainComponents();

            await builder.Build().RunAsync();
        }
    }
}
