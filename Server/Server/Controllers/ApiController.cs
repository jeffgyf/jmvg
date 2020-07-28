using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            sqlPassword = config["Database:Password"];
            if (sqlUsername == null || sqlPassword == null)
            {
                throw new Exception("failed to read username or/and password from config");
            }
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
        public IActionResult GetVideoList(int start, int count)
        {
            var dbContext = new JmvgDbContext(sqlUsername, sqlPassword);
            var videos = dbContext.Videos.FromSqlRaw($"SELECT TOP({count}) * FROM [Video] WHERE VideoId >={start}");

            return Content(JsonConvert.SerializeObject(videos), "application/json");
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
