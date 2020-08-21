using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Server.Models;

namespace Server.Controllers
{
    public class ApiController : Controller
    {
        private ILogger logger;

        public ApiController(ILogger<ApiController> logger)
        {
            this.logger = logger;
        }

        [Route("")]
        [HttpGet]
        public IActionResult HomePage()
        {
            return Redirect("api/ok");
        }

        [Route("api/getVideoList")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetVideoList(int start, int count)
        {
            const int failurePct = 10;
            if (new Random().NextDouble() <= (0.01*failurePct))
            {
                throw new Exception($"Random {failurePct}% failure");
            }

            var dbContext = new JmvgDbContext();
            var videos = dbContext.Videos.FromSqlRaw($"SELECT TOP({count}) * FROM [Video] WHERE VideoId >={start}");

            return Content(JsonConvert.SerializeObject(await videos.ToListAsync()), "application/json");
        }

        [Route("api/getVideoListCpuIntensive")]
        [HttpGet]
        [Produces("application/json")]
        public Task<IActionResult> GetVideoListCpuIntensive(int start, int count, int degree = 8)
        {
            CpuIntensiveFunction(degree);
            return GetVideoList(start, count);
        }

        [Route("api/getVideoListSlow")]
        [HttpGet]
        [Produces("application/json")]
        public Task<IActionResult> GetVideoListSlow(int start, int count, int timeCostInMin = 1)
        {
            Task.Delay(TimeSpan.FromMinutes(timeCostInMin)).Wait();
            return GetVideoList(start, count);
        }

        [Route("api/getVideoListHeavyMemory")]
        [HttpGet]
        [Produces("application/json")]
        public Task<IActionResult> GetVideoListHeavyMemory(int start, int count)
        {
            var l = WasteMemory(100);
            Task.Delay(TimeSpan.FromSeconds(10)).Wait();
            return GetVideoList(start, count);
        }

        [Route("api/getVideoListHeavyMemorySlow")]
        [HttpGet]
        [Produces("application/json")]
        public Task<IActionResult> GetVideoListHeavyMemorySlow(int start, int count, int timeCostInMin = 1)
        {
            WasteMemory(500);
            Task.Delay(TimeSpan.FromMinutes(timeCostInMin)).Wait();
            return GetVideoList(start, count);
        }

        [Route("api/getVideoListHeavyMemoryAndCpu")]
        [HttpGet]
        [Produces("application/json")]
        public Task<IActionResult> GetVideoListHeavyMemoryAndCpu(int start, int count, int degree = 8)
        {
            WasteMemory(800);
            CpuIntensiveFunction(degree);
            return GetVideoList(start, count);
        }

        [Route("api/test")]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Test()
        {
            Program.TryCrash(1, "test");
            return Content("ok");
        }

        [Route("api/crashTest")]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult CrashTest()
        {
            Program.Crash("Crash Test api triggered");

            return Content("ok");
        }

        [Route("api/setCrashTask")]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult SetCrashTask(int? instanceNum = null)
        {
            double restartHour = 1;
            new Timer(s =>
            {
                string msg = $"Try restarting server {Logger.InstanceName} after it runs for {restartHour:0.00} hrs";
                if (instanceNum == null)
                {
                    logger.LogTrace(msg);
                    Program.Crash(msg);
                }
                else
                {
                    TryCrash(instanceNum.Value, msg);
                }
                //Environment.FailFast($"Restarting server after it runs for {randomRestartHour:0.00} hrs", new Exception("Random Crash"));
            }, null, TimeSpan.FromHours(restartHour), Timeout.InfiniteTimeSpan);
            Program.Crash("Crash Test api triggered");

            return Content("ok");
        }


        private void TryCrash(int instanceNum, string msg)
        {
            var dbContext = new JmvgDbContext();

            while (true)
            {
                using (var trans = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        int instanceCnt = dbContext.Instances.Count();
                        logger.LogTrace($"{msg}, there are {instanceCnt} instances online");
                        if (instanceCnt == instanceNum)
                        {
                            dbContext.Instances.Remove(new Instance { Id = Logger.InstanceName });
                            dbContext.SaveChanges();
                            trans.Commit();
                            Program.Crash(msg);
                        }
                        else
                        {
                            logger.LogTrace($"No enough instances online, wait for 5 mins and retry");
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "TryCrash failed");
                    }
                }
                //Task.Delay(TimeSpan.FromMinutes(5)).Wait();
            }
        }

        [Route("api/stackOverflowCrashTest")]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult StackOverFlowCrashTest()
        {
            return StackOverFlowCrashTest();
        }

        [Route("api/exit")]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Exit()
        {
            Environment.Exit(-1);

            return Content("ok");
        }

        [Route("api/ok")]
        [HttpGet]
        public IActionResult Ok()
        {
            return Content("ok");
        }

        [Route("api/getInstanceId")]
        [HttpGet]
        [Produces("text/plain")]
        public IActionResult GetInstanceId()
        {
            return Content(Logger.InstanceName.ToString());
        }

        private double CpuIntensiveFunction(int degree)
        {
            var rand = new Random();
            double t = 1;
            for (long i = 0; i < ((long)1) << (degree+20); ++i)
            {
                if (t > 1000000000)
                {
                    t *= rand.NextDouble();
                }
                else
                {
                    t /= rand.NextDouble();
                }
            }
            return t;
        }

        private List<int[]> WasteMemory(int n)
        {
            var list = new List<int[]>();
            try
            {
                list.Add(new int[n * 256 * 1024]);
                //logger.LogInformation($"{n} MB memory allocated");
                return list;
            }
            catch (Exception e)
            {
                throw new Exception($"failed to allocate {n} MB memory");
            }
        }
    }
}
