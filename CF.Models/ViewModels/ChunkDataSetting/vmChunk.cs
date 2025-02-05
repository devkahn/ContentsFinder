using System.Collections.ObjectModel;
using CF.Models.DataModels.ChunkData;

namespace CF.Models.ViewModels.ChunkDataSetting
{
    public partial class vmChunk
    {
        private mChunk _Origin = null;
    }
    public partial class vmChunk :vmBase
    {
        public vmChunk(mChunk orign)
        {
            SetInitialData();
            this.Origin = orign;
        }

        public mChunk Origin
        {
            get => _Origin;
            set
            {
                _Origin = value;
                if (value == null) return;
               
            }
        }

        public ObservableCollection<vmChunkData> Data { get; private set; }
    }
    public partial class vmChunk
    {
        public override void SetInitialData()
        {
            this.Data = new ObservableCollection<vmChunkData>();    
        }
        public override void UpdateOriginData()
        {

        }
    }

}
