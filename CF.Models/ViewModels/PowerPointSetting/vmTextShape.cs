

using System.Windows;
using System.Windows.Media;
using CF.Models.DataModels;

namespace CF.Models.ViewModels.PowerPointSetting
{
    public partial class vmTextShape 
    {
        private bool _HasContent = false;

        private mTextShape _Origin = null;
        //private vmTableContent _MapContent = null;

        private object _Display_Value = null;
        
    }
    public partial class vmTextShape : vmBase
    {
        public vmTextShape(mTextShape origin)
        {
            SetInitialData();
            this.Origin = origin;   
        }
        public mTextShape Origin
        {
            get => _Origin;
            set
            {
                _Origin = value;
                if (value == null) return;

                this.Display_Value = value.Text;
            }
        }
        //public vmTableContent MapContent
        //{
        //    get => _MapContent;
        //    set
        //    {
        //        _MapContent = value;
        //        this.HasMapContent = value != null;
        //    }
        //}


        public bool HasMapContent
        {
            get => _HasContent;
            private set
            {
                _HasContent = value;
                OnPropertyChanged(nameof(Background));
                OnPropertyChanged(nameof(BorderThickness));
                OnPropertyChanged(nameof(FontWeight));
                OnPropertyChanged(nameof(Foreground));
                OnPropertyChanged(nameof(Margin));
            }
        }
        public Brush Background => this.HasMapContent ? Brushes.Yellow : Brushes.White;
        public Thickness BorderThickness => this.HasMapContent ? new Thickness(3, 0, 0, 1) : new Thickness(0, 0, 0, 1);
        public FontWeight FontWeight => this.HasMapContent ? FontWeights.UltraBold : FontWeights.Normal;
        public Brush Foreground => this.HasMapContent ? Brushes.Black : Brushes.Gray;
        public Thickness Margin => this.HasMapContent ? new Thickness(5) : new Thickness(25, 0, 5, 0);
        public object Display_Value
        {
            get => _Display_Value;
            private set
            {
                _Display_Value = value;
                OnPropertyChanged(nameof(Display_Value));   
            }
        }

    }
    public partial class vmTextShape 
    {
        public override void SetInitialData()
        {

        }
        public override void UpdateOriginData()
        {
            //if (this.HasMapContent) this.Origin.MapContentId = this.MapContent.Origin.Id;
        }
        public void SetDisplayText()
        {
            this.Display_Value = this.Origin.Text;  
        }
    }
}
