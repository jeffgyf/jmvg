using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const bool slowStart = true;

            Logger.Initialize();
            if (slowStart)
            {
                int timeToDelayInMin = 10;
                Logger.TraceInformation($"Start slow start, delay for {timeToDelayInMin} mins");
                Task.Delay(TimeSpan.FromMinutes(10)).Wait();
            }
            Logger.TraceInformation($"Initialization done");

            double randomRestartHour = 1 + (new Random().NextDouble()) * 2;
            new Timer(s =>
            {
                Logger.TraceInformation($"Restarting server after it runs for {randomRestartHour:0.00} hrs");
                Environment.Exit(-1);
            }, null, TimeSpan.FromHours(randomRestartHour), Timeout.InfiniteTimeSpan);


           CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().ConfigureLogging(
                        builder => 
                        {
                            builder.AddApplicationInsights("4db899c8-043a-4a36-b28a-169e6024e0a7");
                        });
                });
    }
}
