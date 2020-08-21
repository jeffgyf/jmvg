using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Server.Models;

namespace Server
{
    public class Program
    {
        public static void Crash(string msg)
        {
            promise.TrySetResult(msg);
        }
        private static TaskCompletionSource<string> promise = new TaskCompletionSource<string>();
        public static void Main(string[] args)
        {
            bool slowStart = !Debugger.IsAttached;

            Logger.Initialize();
            if (slowStart)
            {
                VerySlowStartUp(15);
            }
            Logger.TraceInformation($"Instance {Logger.InstanceName} initialization done");
            SetCrashTask(restartHour: 1, instanceNum: 2);

            var hostTask = CreateHostBuilder(args).Build().RunAsync();
            string msg = promise.Task.Result;
            throw new Exception(msg);
        }

        private static void VerySlowStartUp(int timeToDelayInMin = 10)
        {
            Logger.TraceInformation($"Start slow start, delay for {timeToDelayInMin} mins");
            Task.Delay(TimeSpan.FromMinutes(timeToDelayInMin)).Wait();
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
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddEventSourceLogger();
                });

        public static void TryCrash(int instanceNum, string msg)
        {
            var dbContext = new JmvgDbContext();

            while (true)
            {
                using (var trans = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        int instanceCnt = dbContext.Instances.Count();
                        Logger.TraceInformation($"{msg}, there are {instanceCnt} instances online");
                        if (instanceCnt == instanceNum)
                        {
                            dbContext.Instances.Remove(new Instance { Id = Logger.InstanceName });
                            dbContext.SaveChanges();
                            trans.Commit();
                            Crash(msg);
                            break;
                        }
                        else
                        {
                            Logger.TraceInformation($"No enough instances online, wait for 5 mins and retry");
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.TraceInformation("TryCrash failed, retrying");
                    }
                }
                Task.Delay(TimeSpan.FromMinutes(5)).Wait();
            }
        }

        public static void SetCrashTask(double restartHour = 1, int? instanceNum = null)
        {
            new Timer(s =>
            {
                string msg = $"Try restarting server {Logger.InstanceName} after it runs for {restartHour:0.00} hrs";
                if (instanceNum == null)
                {
                    Logger.TraceInformation(msg);
                    Crash(msg);
                }
                else
                {
                    TryCrash(instanceNum.Value, msg);
                }
                //Environment.FailFast($"Restarting server after it runs for {randomRestartHour:0.00} hrs", new Exception("Random Crash"));
            }, null, TimeSpan.FromHours(restartHour), Timeout.InfiniteTimeSpan);
        }
    }
}
