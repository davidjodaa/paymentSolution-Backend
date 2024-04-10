using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PartyPal;
using NLog;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();
            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .CreateLogger();
            try
            {
                Log.Information("Application Starting...");
                // Licensing.RegisterLicense("8757-e1JlZjo4NzU3LE5hbWU6QWNjZXNzIEJhbmsgUGxjLFR5cGU6QnVzaW5lc3MsTWV0YTowLEhhc2g6a3JXamJvQnpvZnpWbGZReUhmUHUvd0RsRmMzcFhaSUJpd0FVNy81M2FLV3RoelpUQnNjMDRYSktDdEN3bk51cWNONXdLek1YeCtCZFM2cGFKMzBkZG5xaGZWQWExeUVKMmZaUXMrSnJKOXZsWTZnWFNQU1RZeTArcytNUWlNRGV1aEh0bmVCQzc2ZVF2RDVwQjBIby84bGxpa2d6cHAxYkczRjFSbUFSRmE4PSxFeHBpcnk6MjAyMS0xMi0yMX0=");
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
