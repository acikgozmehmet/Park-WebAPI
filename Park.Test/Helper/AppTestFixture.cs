using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Park.Test.Helper
{
    //public static class Helper
    //{

    /// Usage on the 

    //->>>> This one is important        https://blog.markvincze.com/overriding-configuration-in-asp-net-core-integration-tests/

    //  https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0&viewFallbackFrom=aspnetcore-3.0#basic-tests-with-the-default-webapplicationfactory
    ///

/*
    /// <summary>
    /// Create Host Builder
    /// </summary>
    //}
    public class AppTestFixture : WebApplicationFactory<Startup>
    {
*//*        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder().
                ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseTestServer();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                });

            return builder;
        }*//*



        //protected override IHostBuilder CreateHostBuilder()
        //{
        //    var builder = Host.CreateDefaultBuilder().
        //        ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        })
        //        .ConfigureAppConfiguration((hostingContext, config) =>
        //        {
        //            var settings = config.Build();
        //            var url = settings.GetSection("Park").GetSection("applicationUrl").Value;
        //                //config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
        //        });

        //    return builder;
        //}



    }
*/

}
