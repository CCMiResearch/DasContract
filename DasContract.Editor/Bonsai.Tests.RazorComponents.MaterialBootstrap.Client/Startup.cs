using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Bonsai.RazorComponents.MaterialBootstrap.Services;

namespace Bonsai.Tests.RazorComponents.MaterialBootstrap.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMaterialBootstrap();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
