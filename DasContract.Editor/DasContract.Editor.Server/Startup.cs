using Bonsai.RazorPages.Error.Services.LanguageDictionary;
using Bonsai.RazorPages.Error.Services.LanguageDictionary.Languages;
using Bonsai.Services.Interfaces;
using DasContract.Editor.AppLogic.Facades;
using DasContract.Editor.AppLogic.Facades.Interfaces;
using DasContract.Editor.DataPersistence.DbContexts;
using DasContract.Editor.DataPersistence.Repositories;
using DasContract.Editor.DataPersistence.Repositories.Interfaces;
using DasContract.Editor.Pages.Main.Services.FilePathProvider.SpecificFilePathProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

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

        public void ConfigureServices(IServiceCollection services)
        {
            //Add controllers
            services.AddMvc();
            services.AddControllers();

            //Add contract editor db
            if(Environment.IsDevelopment())
            {
                services.AddDbContext<ContractEditorDb>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ContractEditorDbLocal")));
            }
            else
            {
                services.AddDbContext<ContractEditorDb>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("ContractEditorDbSQLite")));

                using var serviceScope = services.BuildServiceProvider().CreateScope();
                var dbContext = serviceScope.ServiceProvider.GetService<ContractEditorDb>();
                dbContext.Database.EnsureCreated();
            }

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

            //File provider
            if (Environment.IsDevelopment())
                services.AddSingleton<IFilePathProvider, RegularFilePathProvider>();
            else
                services.AddSingleton<IFilePathProvider, VersionedFilePathProvider>();

            //Error pages services
            services.AddSingleton<IErrorLanguageDictionary, EnglishErrorLanguageDictionary>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/error", "?code={0}");
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseClientSideBlazorFiles<Pages.Main.Program>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapFallbackToClientSideBlazor<Pages.Main.Program>("index.html");
            });
        }
    }
}
