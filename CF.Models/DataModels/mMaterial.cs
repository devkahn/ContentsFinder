using CF.Models.DataModels.ChunkData;
using CF.Models.DataModels.Markdown;
using CF.Models.DataModels.Powerpoint;

namespace CF.Models.DataModels
{
    public class mMaterial :mBase
    {
        public mMaterial()
        {

        }


        public string Name { get; set; } = string.Empty;

        public mPowerpoint PowerPoint { get; set; } = new mPowerpoint();
        public mMarkdown Markdown { get; set; } = new mMarkdown();
        public mChunk Chunk { get; set; } = new mChunk();
    }
}
