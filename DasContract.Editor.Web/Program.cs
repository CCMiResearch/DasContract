using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DasContract.Editor.Web.Services.BpmnEvents;
using DasContract.Editor.Web.Services;
using DasContract.Editor.Web.Services.EditElement;
using DasContract.Editor.Web.Services.Processes;

namespace DasContract.Editor.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<IBpmnEventHandler, BpmnEventHandler>();
            builder.Services.AddScoped<IContractManager, ContractManager>();
            builder.Services.AddScoped<IProcessManager, ProcessManager>();
            builder.Services.AddScoped<IBpmnSynchronizer, BpmnSynchronizer>();
            builder.Services.AddScoped<EditElementService>();

            var host =  builder.Build();
            var synchronizerService = host.Services.GetRequiredService<IBpmnSynchronizer>();
            synchronizerService.Initiliaze();

            await host.RunAsync();
        }
    }
}
