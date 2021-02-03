using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalMemory_WebApi_3_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        }).ConfigureServices((context, services) =>
        //        {

        //        });

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = new HostBuilder();

            builder.ConfigureHostConfiguration(config =>
            {
                // Uses DOTNET_ environment variables and command line args
            });

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                // JSON files, User secrets, environment variables and command line arguments
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                // Adds loggers for console, debug, event source, and EventLog (Windows only)
            })
            .UseDefaultServiceProvider((context, options) =>
            {
                // Configures DI provider validation
            }).ConfigureServices((a,b)=> {
            
            });

            return builder;
        }
    }
}
