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

namespace DasContract.Editor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Add contract editor db
            services.AddDbContext<ContractEditorDb>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ContractEditorDbLocal")));

            //Add contract editor services
            services.AddTransient<IContractFileSessionRepository, ContractFileSessionRepository>();
            services.AddTransient<IContractFileSessionFacade, ContractFileSessionFacade>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
