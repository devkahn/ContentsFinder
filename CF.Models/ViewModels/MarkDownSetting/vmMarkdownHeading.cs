using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using CF.Commons;
using CF.Models.DataModels;
using CF.Models.DataModels.Markdown;

namespace CF.Models.ViewModels.MarkDownSetting
{
    public partial class vmMarkdownHeading
    {
        private vmMarkdownLine _ViewModelOrigin = null;

        private bool _IsHighLight = false;

        private object _Display_Text = null;

    }
    public partial class vmMarkdownHeading :vmBase
    {
        public vmMarkdownHeading(vmMarkdownLine origin)
        {
            SetInitialData();
            this.Origin = origin;
        }
        public vmMarkdownLine Origin
        {
            get => _ViewModelOrigin;
            set
            {
                _ViewModelOrigin = value;
                if (value == null) return;

                this.RawOrigin = value.Origin;

                this.Display_Text = value.Display_LineValue;
            }
        }

        /* Status */
        public eLineType HeadingType => this.Origin == null? eLineType.None : this.Origin.LineType;
        public bool IsHighLight
        {
            get => _IsHighLight;
            set
            {
                _IsHighLight = value;
                OnPropertyChanged(nameof(Background));
                OnPropertyChanged(nameof(Foreground));
                OnPropertyChanged(nameof(FontWeight));
                OnPropertyChanged(nameof(Italic));
            }
        }

        /* Variable */
        public mMarkdownLine RawOrigin { get; private set; }
        public vmMarkdownHeading ParentHeading { get; private set; } = null;
        public ObservableCollection<vmMarkdownHeading> Children { get; private set; } = null;
        public vmMarkdownContent Conetnt { get; private set; } = null;

        /* Style */
        public Brush Background => this.IsHighLight ? Brushes.Yellow : Brushes.Transparent;
        public Brush Foreground => this.IsHighLight ? Brushes.Black : Brushes.Gray;
        public FontWeight FontWeight => this.IsHighLight ? FontWeights.UltraBold : FontWeights.Normal;
        public bool Italic => this.IsHighLight;


        /* Display */
        public object Display_Text
        {
            get => _Display_Text;
            set
            {
                _Display_Text = value;
                OnPropertyChanged(nameof(Display_Text));
            }
        }

    }
    public partial class vmMarkdownHeading
    {
        public override void SetInitialData()
        {
            this.Children = new ObservableCollection<vmMarkdownHeading>();
            this.Children.CollectionChanged += Children_CollectionChanged;
        }
        public override void UpdateOriginData()
        {

        }

        public void SetConent(mMarkdownContent content)
        {
            if (content == null) content = new mMarkdownContent();
            this.Conetnt = new vmMarkdownContent(content);
            this.Conetnt.SetParentHeading(this);
        }


        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (e.NewItems == null) return;
                    foreach (var item in e.NewItems)
                    {
                        vmMarkdownHeading addedItem = item as vmMarkdownHeading;
                        if(addedItem == null) continue;

                        addedItem.ParentHeading = this;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if(e.OldItems == null) return; 
                    foreach (var item in e.OldItems)
                    {
                        vmMarkdownHeading removedItem = item as vmMarkdownHeading;
                        if(removedItem == null) continue;

                        removedItem.ParentHeading = null;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                default:
                    break;
            }
        }

        
    }
}
