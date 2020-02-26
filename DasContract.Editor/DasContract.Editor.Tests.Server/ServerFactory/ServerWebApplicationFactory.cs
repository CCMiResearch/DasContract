using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using DasContract.Editor.DataPersistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DasContract.Editor.Server;

namespace DasContract.Editor.Tests.Server.ServerFactory
{
    public class ServerWebApplicationFactory
        : WebApplicationFactory<Startup>
    {
        readonly ContractEditorDbTestBuilder dbBuilder = new ContractEditorDbTestBuilder();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //Remove the original ContractEditorDb
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ContractEditorDb>));
                if (descriptor != null)
                    services.Remove(descriptor);

                //Add sqlite database
                services.AddTransient<DbContextOptions<ContractEditorDb>>(e => null);
                services.AddTransient(e => dbBuilder.Build());

                /*//Add InMemory database
                services.AddDbContext<ContractEditorDb>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting-" + Guid.NewGuid().ToString());
                });

                //Build the service provider.
                var sp = services.BuildServiceProvider();

                //Create a scope to obtain a reference to the database context
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ContractEditorDb>();

                //Ensure the database is created.
                db.Database.EnsureCreated();
                db.SeedTests();*/
                
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            dbBuilder.Dispose();
        }
    }
}
