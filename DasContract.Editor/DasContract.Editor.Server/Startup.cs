using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.DataPersistence.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DasContract.Editor.DataPersistence.Repositories;
using DasContract.Editor.DataPersistence.Repositories.Interfaces;
using DasContract.Editor.AppLogic.Facades;
using DasContract.Editor.AppLogic.Facades.Interfaces;
using Microsoft.AspNetCore.ResponseCompression;
using Bonsai.RazorComponents.MaterialBootstrap.Services;

namespace DasContract.Editor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add API controllers
            services.AddControllers();

            //Add contract editor db
            services.AddDbContext<ContractEditorDb>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ContractEditorDbLocal")));

            //Add contract editor services
            services.AddTransient<IContractFileSessionRepository, ContractFileSessionRepository>();
            services.AddTransient<IContractFileSessionFacade, ContractFileSessionFacade>();

            //Controllers view builder
            var controllersViewBuilder = services.AddControllersWithViews();
            if (Environment.IsDevelopment())
                controllersViewBuilder.AddRazorRuntimeCompilation();

            //Razor pages builder
            var razorPagesBuilder = services.AddRazorPages();
            if (Environment.IsDevelopment())
                razorPagesBuilder.AddRazorRuntimeCompilation();

            //HTTPS
            services.AddHttpsRedirection(options => options.HttpsPort = 443);

            //Response compression
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            //Material bootstrap
            services.AddMaterialBootstrap();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }
                
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseClientSideBlazorFiles<Pages.Main.Program>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToClientSideBlazor<Pages.Main.Program>("index.html");
            });
        }
    }
}
