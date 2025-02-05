using System.Collections.Generic;
using System.Collections.ObjectModel;
using CF.Models.DataModels.ChunkData;

namespace CF.Models.ViewModels.ChunkDataSetting
{
    public partial class vmChunkData :vmBase
    {
        private mChunkData _Origin = null;

        private object _Display_DocType = null;
        private object _Display_DocLevel1 = null;
        private object _Display_DocLevel2 = null;
        private object _Display_DocLevel3 = null;
        private object _Display_DocLevel4 = null;
        private object _Display_DocLevel5 = null;
        private object _Display_DocLevel6 = null;
        private object _Display_ManualName = null;
        private object _Display_MainKeyword = null;
        private object _Display_Title = null;
        private object _Display_EmbeddingContents = null;
        private object _Display_RetrieveContents = null;
        private object _Display_GenerationContents = null;
        private object _Display_ReferenceContents = null;
        private object _Display_Vertors = null;

    }
    public partial class vmChunkData : vmBase
    {
        public vmChunkData (mChunkData origin)
        {
            SetInitialData();
            this.Origin = origin;
        }

        public mChunkData Origin
        {
            get => _Origin;
            private set
            {
                _Origin = value;
                if (value == null) return;

                this.Display_DocType = value.DocType;
                this.Display_DocLevel1 = value.DocLevel1 == null ? null : value.DocLevel1.Trim();
                this.Display_DocLevel2 = value.DocLevel2 == null ? null : value.DocLevel2.Trim();
                this.Display_DocLevel3 = value.DocLevel3 == null ? null : value.DocLevel3.Trim();
                this.Display_DocLevel4 = value.DocLevel4 == null ? null : value.DocLevel4.Trim();
                this.Display_DocLevel5 = value.DocLevel5 == null ? null : value.DocLevel5.Trim();
                this.Display_DocLevel6 = value.DocLevel6 == null ? null : value.DocLevel6.Trim();
                this.Display_ManualName = value.ManualName == null? null : value.ManualName.Trim();
                this.Display_MainKeyword = value.MainKeyword == null ? null : value.MainKeyword.Trim();
                this.Display_Title = value.Title == null ? null : value.Title.Trim();
                this.Display_EmbeddingContents = value.EmbeddingContents == null ? null : value.EmbeddingContents.Trim();
                this.Display_RetrieveContents = value.RetrieveContents == null ? null : value.RetrieveContents.Trim();
                this.Display_GenerationContents = value.GenerationContents == null ? null : value.GenerationContents.Trim();
                this.Display_ReferenceContents = value.ReferenceContents == null ? null : value.ReferenceContents.Trim();
                this.Display_Vertors = SerializeVertors(value.Vertors);  

                SetChunkImages(value.ImageLinks);
            }
        }



        public ObservableCollection<vmChunkImage> Images { get; private set; } = null;

        public object Display_DocType 
        {
            get => _Display_DocType;
            private set
            {
                _Display_DocType = value;
                OnPropertyChanged(nameof(Display_DocType)); 
            }
        }
        public object Display_DocLevel1
        {
            get => _Display_DocLevel1;
            private set
            {
                _Display_DocLevel1 = value;
                OnPropertyChanged(nameof(Display_DocLevel1));
            }
        }
        public object Display_DocLevel2
        {
            get => _Display_DocLevel2;
            private set
            {
                _Display_DocLevel2 = value;
                OnPropertyChanged(nameof(Display_DocLevel2));
            }
        }
        public object Display_DocLevel3
        {
            get => _Display_DocLevel3;
            private set
            {
                _Display_DocLevel3 = value;
                OnPropertyChanged(nameof(Display_DocLevel3));
            }
        }
        public object Display_DocLevel4
        {
            get => _Display_DocLevel4;
            private set
            {
                _Display_DocLevel4 = value;
                OnPropertyChanged(nameof(Display_DocLevel4));
            }
        }
        public object Display_DocLevel5
        {
            get => _Display_DocLevel5;
            private set
            {
                _Display_DocLevel5 = value;
                OnPropertyChanged(nameof(Display_DocLevel5));
            }
        }
        public object Display_DocLevel6
        {
            get => _Display_DocLevel6;
            private set
            {
                _Display_DocLevel6 = value;
                OnPropertyChanged(nameof(Display_DocLevel6));
            }
        }
        public object Display_ManualName
        {
            get => _Display_ManualName;
            private set
            {
                _Display_ManualName = value;
                OnPropertyChanged(nameof(Display_ManualName));
            }
        }
        public object Display_MainKeyword
        {
            get => _Display_MainKeyword;
            private set
            {
                _Display_MainKeyword = value;
                OnPropertyChanged(nameof(Display_MainKeyword));
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
        public object Display_EmbeddingContents
        {
            get => _Display_EmbeddingContents;
            private set
            {
                _Display_EmbeddingContents = value;
                OnPropertyChanged(nameof(Display_EmbeddingContents));
            }
        }
        public object Display_RetrieveContents
        {
            get => _Display_RetrieveContents;
            private set
            {
                _Display_RetrieveContents = value;
                OnPropertyChanged(nameof(Display_RetrieveContents));
            }
        }
        public object Display_GenerationContents
        {
            get => _Display_GenerationContents;
            private set
            {
                _Display_GenerationContents = value;
                OnPropertyChanged(nameof(Display_GenerationContents));
            }
        }
        public object Display_ReferenceContents
        {
            get => _Display_ReferenceContents;
            private set
            {
                _Display_ReferenceContents = value;
                OnPropertyChanged(nameof(Display_ReferenceContents));
            }
        }
        public object Display_Vertors
        {
            get => _Display_Vertors;
            private set
            {
                _Display_Vertors = value;
                OnPropertyChanged(nameof(Display_Vertors));
            }
        }

    }
    public partial class vmChunkData 
    {
        public override void SetInitialData()
        {
            this.Images = new ObservableCollection<vmChunkImage>(); 
        }
        public override void UpdateOriginData()
        {

        }
        private object SerializeVertors(List<double> vertors)
        {
            if (vertors == null) return string.Empty;

            string output = string.Empty;

            foreach (double v in vertors)
            {
                output += ",";
                output += v.ToString();
            }

            return output.Substring(1);
        }
        private void SetChunkImages(List<mChunkImage> imageLinks)
        {
            if (this.Images == null) this.Images = new ObservableCollection<vmChunkImage>();

            foreach (mChunkImage image in imageLinks)
            {
                vmChunkImage newImage = new vmChunkImage(image);
                newImage.SetParent(this);
                this.Images.Add(newImage);  
            }
        }
    }
}
