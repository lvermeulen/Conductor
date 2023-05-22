using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Conductor.Abstractions;
using Conductor.Api.Extensions;

namespace Conductor.Api
{
    public static class Program
    {
        private static readonly ApplicationVersionInformation s_applicationVersionInformation = new ApplicationVersionInformation();

        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();
            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ConfigureSerilog(context, services, "Conductor.Api", s_applicationVersionInformation.GetBuildInformation()))
                .ConfigureWebHost(hostBuilder => hostBuilder
                    .ConfigureKestrel(options => options.AddServerHeader = false))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
