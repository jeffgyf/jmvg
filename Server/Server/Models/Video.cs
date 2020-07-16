using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Server.Models
{
    [Table("Video")]
    public class Video
    {
        public string Title { get; set; }

        [JsonIgnore]
        [Column("VideoInfo")]
        public string VideoInfoStr { get; set; }

        [NotMapped]
        public IEnumerable<string> VideoInfo => VideoInfoStr?.Split(';');

        public string CoverImg { get; set; }

        [JsonIgnore]
        [Column("Tags")]
        public string TagStr { get; set; }

        [NotMapped]
        public IEnumerable<string> Tags => TagStr?.Split(';');

        [Key]
        public int VideoId { get; set; }
        public string VideoPath { get; set; }
    }
}
