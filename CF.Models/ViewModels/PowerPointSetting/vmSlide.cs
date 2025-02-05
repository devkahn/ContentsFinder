using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CF.Commons;
using CF.Models.DataModels;


namespace CF.Models.ViewModels.PowerPointSetting
{
    public partial class vmSlide
    {
        private mSlide _Origin = null;

        private eCheckStatus _CheckStatus = eCheckStatus.None;

        private object _Display_Name = null;
    }
    public partial class vmSlide : vmBase
    {
        public vmSlide(mSlide origin)
        {
            SetInitialData();
            this.Origin = origin;
        }

        public mSlide Origin
        {
            get => _Origin;
            private set
            {
                _Origin = value;
                if (value == null) return;

                this.Display_Name = value.Index;
                foreach (mTextShape txtShape in value.TextShapes)
                {
                    vmTextShape newSlide = new vmTextShape(txtShape);
                    this.Shapes.Add(newSlide);
                }
            }
        }

        public vmPowerpoint ParentPowerpoint { get; set; } = null;
        public ObservableCollection<vmTextShape> Shapes { get; private set; }



        public eCheckStatus CheckStatus
        {
            get => _CheckStatus;
            private set
            {
                _CheckStatus = value;
                OnPropertyChanged(nameof(BadgeVisivility));
                OnPropertyChanged(nameof(BadgeBakcground));
                OnPropertyChanged(nameof(BadgeText));
            }
        }
        public Visibility BadgeVisivility
        {
            get
            {
                switch (this.CheckStatus)
                {
                    case eCheckStatus.Completed: 
                    case eCheckStatus.Hold: 
                    case eCheckStatus.Fail: 
                        return Visibility.Visible;
                    default: return Visibility.Collapsed;
                }
            }
        }
        public Brush BadgeBakcground
        {
            get
            {
                switch (this.CheckStatus)
                {
                    case eCheckStatus.Completed: return Brushes.DarkGreen;
                    case eCheckStatus.Hold: return Brushes.Goldenrod;
                    case eCheckStatus.Fail: return Brushes.DarkRed;
                    default: return Brushes.Transparent;
                }
            }
        }
        public string BadgeText
        {
            get
            {
                switch (this.CheckStatus)
                {
                    case eCheckStatus.Completed: return "S";
                    case eCheckStatus.Hold: return "H";
                    case eCheckStatus.Fail: return "F";
                    default: return string.Empty;
                }
            }
        }

        public object Display_Name
        {
            get => _Display_Name;
            private set
            {
                _Display_Name = value;
                OnPropertyChanged(nameof(Display_Name));
            }
        }
        public int Display_ShapeCnt => this.Shapes.Count;
    }
    public partial class vmSlide
    {
        public override void SetInitialData()
        {
            this.Shapes = new ObservableCollection<vmTextShape>();
            this.Shapes.CollectionChanged += TextShape_CollectionChanged;
        }
        public override void UpdateOriginData()
        {
            this.Origin.CheckStatus = this.CheckStatus.GetHashCode();
            foreach (vmTextShape item in this.Shapes)
            {
                item.Origin.ParentId = this.Origin.Id;
                item.UpdateOriginData();
            }
        }
        public void SetCheckStatus(eCheckStatus status)
        {
            this.CheckStatus = status;
        }
       
        private void TextShape_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    
                    break;
                default:
                    break;
            }
        }
    }

}
