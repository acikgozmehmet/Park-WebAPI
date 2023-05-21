using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Park.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Park.Test.Mock
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {                    
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();


                    try
                    {
                        MockUtilities.InitializeDbForTests(db);
                    }
                    catch  (Exception ex)
                    {
                        logger.LogError(ex, "An error occured seeding the database with the test messages. Error : {Message}", ex.Message);

                    }
                }
            });
            //base.ConfigureWebHost(builder);
        }
    }
}