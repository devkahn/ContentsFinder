using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Models.DataModels.ChunkData
{
    public class mChunk : mBase
    {
        public List<mChunkData> Datas { get; set; } = new List<mChunkData>();
    }
}
