using CF.Models.DataModels;
using CF.Models.ViewModels.ChunkDataSetting;
using CF.Models.ViewModels.MarkDownSetting;
using CF.Models.ViewModels.PowerPointSetting;

namespace CF.Models.ViewModels
{
    public partial class vmMaterial
    {
        private mMaterial _Origin = null;


    }
    public partial class vmMaterial : vmBase
    {
        public vmMaterial(mMaterial origin)
        {
            SetInitialData();
            this.Origin = origin;
        }


        public mMaterial Origin
        {
            get => _Origin;
            set
            {
                _Origin = value;
                if (value == null) return;

                if(value.PowerPoint != null) this.PowerPoint = new vmPowerpoint(value.PowerPoint);
                if(value.Markdown != null) this.Markdown = new vmMarkdown(value.Markdown);
                if(value.Chunk != null) this.Chunk = new vmChunk(value.Chunk);
            }
        }

        public vmPowerpoint PowerPoint { get; private set; } = null;
        public vmMarkdown Markdown { get; private set; } = null;
        public vmChunk Chunk { get; private set; } = null;  
    }
    public partial class vmMaterial
    {
        public override void SetInitialData()
        {

        }
        public override void UpdateOriginData()
        {
            this.PowerPoint.UpdateOriginData();
            this.Markdown.UpdateOriginData();
            this.Chunk.UpdateOriginData();
        }
        internal void SetPowerPoint(vmPowerpoint value)
        {
            this.PowerPoint = value;
        }
        internal void SetMarkdown(vmMarkdown markdown)
        {
            this.Markdown = markdown;
        }
        internal void SetChunk(vmChunk chunk)
        {
            this.Chunk = chunk; 
        }

    }

}
