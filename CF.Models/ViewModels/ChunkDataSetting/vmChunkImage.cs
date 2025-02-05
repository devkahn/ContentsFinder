using CF.Models.DataModels.ChunkData;

namespace CF.Models.ViewModels.ChunkDataSetting
{
    public partial class vmChunkImage
    {
        private mChunkImage _Origin = null;

        private object _Display_Name = null;
        private object _Display_URL = null;
        private object _Display_Title = null;
        private object _Display_Description = null;


    }
    public partial class vmChunkImage : vmBase
    {
        public vmChunkImage(mChunkImage origin)
        {
            SetInitialData();
            this.Origin = origin;
        }

        public  mChunkImage Origin
        {
            get => _Origin;
            private set
            {
                _Origin = value;
                if (value == null) return;

                this.Display_Name = value.Name;
                this.Display_URL = value.URL;
                this.Display_Title = value.Title;   
                this.Display_Description = value.Description;
            }
        }
        public vmChunkData ParentChunk { get; private set; }


        public object Display_Name
        {
            get => _Display_Name;
            private set
            {
                _Display_Name = value;
                OnPropertyChanged(nameof(Display_Name));
            }
        }
        public object Display_URL
        {
            get => _Display_URL;
            private set
            {
                _Display_URL = value;
                OnPropertyChanged(nameof(Display_URL));
            }
        }
        public object Display_Title
        {
            get => _Display_Title;
            private set
            {
                _Display_Title = value;
                OnPropertyChanged(nameof(Display_Title));
            }
        }
        public object Display_Description
        {
            get => _Display_Description;
            private set
            {
                _Display_Description = value;
                OnPropertyChanged(nameof(Display_Description)); 
            }
        }
    }
    public partial class vmChunkImage
    {
        public override void SetInitialData()
        {
            
        }
        public override void UpdateOriginData()
        {

        }
        public void SetParent(vmChunkData parent)
        {
            this.ParentChunk = parent;
        }
    }
}
