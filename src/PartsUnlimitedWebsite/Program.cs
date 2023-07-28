using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PartsUnlimited.Models;
using Microsoft.Extensions.Hosting;

namespace PartsUnlimited
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args).Build();
            //DateTime now = DateTime.Now;
            //currentDateTime = now.ToString("yyyy-MM-dd hh:mm:ss");
            //string releaseVersion = "1.1";
            //string currentDateTime = "20-07-2023";
            //string buildVersion = "20-07-2023";
            //string releaseProduct = "AGIT-DCCS-ITSM";
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //Version version = assembly.GetName().Version;

            // Convert the Version object to a string
            //string versionString = version.ToString();
            //test lagi
            // Set environment variables
            //Environment.SetEnvironmentVariable("DT_RELEASE_VERSION", releaseVersion);
            //Environment.SetEnvironmentVariable("DT_RELEASE_BUILD_VERSION", versionString);
            //Environment.SetEnvironmentVariable("DT_RELEASE_PRODUCT", releaseProduct);
            Environment.SetEnvironmentVariable("DT_RELEASE_VERSION", "1.1");
            Environment.SetEnvironmentVariable("DT_RELEASE_BUILD_VERSION", "20-07-2023");
            Environment.SetEnvironmentVariable("DT_RELEASE_PRODUCT", "AGIT-DCCS-ITSM");

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    PartsUnlimitedContext context = services.GetRequiredService<PartsUnlimitedContext>();
                    context.Database.EnsureCreated();
                    //Populates the PartsUnlimited sample data
                    SampleData.InitializePartsUnlimitedDatabaseAsync(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }


        public static IHostBuilder BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostContext, config) =>
                {
                    // This is the only configuration file used, development and production.
                    // ENHANCEMENT: Conditionally check the environment variable and load the config based on the environment.
                    config.AddJsonFile("config.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });   
        
       
    }
}
