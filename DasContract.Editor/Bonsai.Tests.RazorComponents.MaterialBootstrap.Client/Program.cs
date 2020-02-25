using System.Threading.Tasks;
using Bonsai.RazorComponents.MaterialBootstrap.Services;
using Microsoft.AspNetCore.Blazor.Hosting;

namespace Bonsai.Tests.RazorComponents.MaterialBootstrap.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddMaterialBootstrap();

            await builder.Build().RunAsync();
        }
    }
}
