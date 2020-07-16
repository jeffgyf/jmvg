using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.Models;

namespace Server.Controllers
{
    public class Foo
    {
        public int A { get; set; }
        public string B { get; set; }
        public Dictionary<string, string> D { get; set; } = new Dictionary<string, string>();
    }
    public class ApiController : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult HomePage()
        {
            return Redirect("index.html?route=videoPage");
        }

        [Route("api/getVideoList")]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetVideoList()
        {
            var sampleVideo = new Video
            {
                Title="Become Wind",
                CoverImg= $"https://{HttpContext.Request.Host}/image/SampleCover.png",
                VideoPath = $"https://{HttpContext.Request.Host}/video/become_wind.mp4",
                VideoId =123
            };
            return Content(JsonConvert.SerializeObject(new [] { sampleVideo }), "application/json");
        }
    }
}
