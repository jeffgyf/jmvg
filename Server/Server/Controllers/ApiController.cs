using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly string sqlUsername;
        private readonly string sqlPassword;

        public ApiController(IConfiguration config)
        {
            sqlUsername = config["Database:Username"];
            string sqlPwdSecret = config["Database:PasswordKvSecretName"];
            string KvUri = config["KvUri"];
            
            if (sqlUsername == null || sqlPwdSecret == null || KvUri == null)
            {
                throw new Exception("failed to read KvUri or/and Username or/and PasswordKvSecretName from config");
            }
            var client = new SecretClient(new Uri(KvUri), new DefaultAzureCredential());
            sqlPassword = client.GetSecret(sqlPwdSecret).Value.Value;
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
            var dbContext = new JmvgDbContext(sqlUsername, sqlPassword);
            var videos = dbContext.Videos.FromSqlRaw($"SELECT TOP({count}) * FROM [Video] WHERE VideoId >={start}");

            return Content(JsonConvert.SerializeObject(await videos.ToListAsync()), "application/json");
        }

        [Route("api/test")]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Test()
        {
            var sampleVideo = new Video
            {
                Title = "Become Wind",
                CoverImg = $"https://{HttpContext.Request.Host}/image/SampleCover.png",
                VideoPath = $"https://{HttpContext.Request.Host}/video/become_wind.mp4",
                VideoId = 123
            };
            return Content(JsonConvert.SerializeObject(sampleVideo), "application/json");
        }
    }
}
