using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            return Redirect("index.html?route=videoPage");
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
            WasteMemory(40);
            return GetVideoList(start, count);
        }

        [Route("api/getVideoListHeavyMemorySlow")]
        [HttpGet]
        [Produces("application/json")]
        public Task<IActionResult> GetVideoListHeavyMemorySlow(int start, int count, int timeCostInMin = 1)
        {
            WasteMemory(40);
            Task.Delay(TimeSpan.FromMinutes(timeCostInMin)).Wait();
            return GetVideoList(start, count);
        }

        [Route("api/getVideoListHeavyMemoryAndCpu")]
        [HttpGet]
        [Produces("application/json")]
        public Task<IActionResult> GetVideoListHeavyMemoryAndCpu(int start, int count, int degree = 8)
        {
            WasteMemory(300);
            CpuIntensiveFunction(degree);
            return GetVideoList(start, count);
        }

        [Route("api/test")]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Test(int n)
        {
            Environment.Exit(-1);

            return Content("ok");
        }

        [Route("api/exit")]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Exit()
        {
            Environment.Exit(-1);

            return Content("ok");
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

        private void WasteMemory(int n)
        {
            var list = new List<int[]>();
            try
            {
                list.Add(new int[n * 256 * 1024]);
                //logger.LogInformation($"{n} MB memory allocated");
            }
            catch (Exception e)
            {
                throw new Exception($"failed to allocate {n} MB memory");
            }
        }
    }
}
