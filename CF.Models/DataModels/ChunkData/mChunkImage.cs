using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CF.Models.DataModels.ChunkData
{
    public class mChunkImage : mBase
    {
        [JsonProperty(Order =0)]
        [Description("이미지 파일명")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(Order = 1)]
        [Description("Manual works URL")]
        public string URL { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
