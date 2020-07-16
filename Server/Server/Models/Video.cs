using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Video
    {
        public string Title;

        [JsonIgnore]
        public string VideoInfoStr;
        public IEnumerable<string> VideoInfo => VideoInfoStr?.Split(';');

        public string CoverImg;
        [JsonIgnore]
        public string TagStr;

        public IEnumerable<string> Tags => TagStr?.Split(';');
        public int VideoId;
        public string VideoPath;
    }
}
