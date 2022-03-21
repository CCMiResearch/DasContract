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
using BlazorPro.BlazorSize;
using DasContract.Editor.Web.Services.Converter;
using DasContract.Editor.Web.Services.UndoRedo;
using DasContract.Editor.Web.Services.UserInput;
using DasContract.Editor.Web.Services.UserForm;
using Blazored.LocalStorage;
using DasContract.Editor.Web.Services.JsInterop;
using DasContract.Editor.Web.Services.LocalStorage;

namespace DasContract.Editor.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<IBpmnEventHandler, BpmnEventHandler>();
            builder.Services.AddScoped<IContractManager, ContractManager>();
            builder.Services.AddScoped<IProcessManager, ProcessManager>();
            builder.Services.AddScoped<IBpmnSynchronizer, BpmnSynchronizer>();
            builder.Services.AddScoped<IBpmnJsCommunicator, BpmnJsCommunicator>();
            builder.Services.AddScoped<ISaveGuardJsCommunicator, SaveGuardJsCommunicator>();
            builder.Services.AddScoped<IContractStorage, ContractStorage>();
            builder.Services.AddScoped<UserInputHandler>();
            builder.Services.AddScoped<EditElementService>();
            builder.Services.AddScoped<ResizeHandler>();
            builder.Services.AddScoped<ResizeListener>();
            builder.Services.AddScoped<SaveManager>();
            builder.Services.AddScoped<UsersRolesManager>();
            builder.Services.AddScoped<UserFormService>();
            builder.Services.AddScoped<IConverterService, ConverterService>();

            var host = builder.Build();

            var contractManager = host.Services.GetRequiredService<IContractManager>();
            await contractManager.InitAsync();

            await host.RunAsync();
        }
    }
}
