using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Commons;
using CF.Models.DataModels.Markdown;


namespace CF.Models.ViewModels.MarkDownSetting
{
    public partial class vmMarkdownContent
    {
        private mMarkdownContent _Origin = null;

        public eCheckStatus _CheckStatus = eCheckStatus.None;

        private object _Display_PageNum = -1;
        private object _Display_Heading1 = "-";
        private object _Display_Heading2 = "-";
        private object _Display_Heading3 = "-";
        private object _Display_Heading4 = "-";
        private object _Display_Heading5 = "-";
        private object _Display_Heading6 = "-";
        private object _Display_Heading7 = "-";
        private object _Display_Content = null;
    }
    public partial class vmMarkdownContent : vmBase
    {
        public vmMarkdownContent(mMarkdownContent origin)
        {
            SetInitialData();
            this.Origin = origin;
        }

        /* Status */
        public mMarkdownContent Origin
        {
            get => _Origin;
            private set
            {
                _Origin = value;
                if (value == null) return;

                this.CheckStatus = (eCheckStatus)value.CheckStatusCode;
                this.Display_PageNum = value.Page;
                this.StartLineNumber = value.StartLineNumber;
                this.EndLineNumber = value.EndLineNumber;
            }
        }

        public eCheckStatus CheckStatus
        {
            get => _CheckStatus;
            private set
            {
                _CheckStatus = value;
            }
        }

        /* Variable */
        public int StartLineNumber { get; private set; } = -1;
        public int EndLineNumber { get; private set; } = -1;
        public ObservableCollection<vmMarkdownLine> Children { get; private set; }
        public vmMarkdownHeading Parent { get; private set; }
        


        /* Display */
        public object Display_PageNum
        {
            get => _Display_PageNum;
            set
            {
                _Display_PageNum = value;
                OnPropertyChanged(nameof(Display_PageNum));
            }
        }
        public object Display_Heading1
        {
            get => _Display_Heading1;
            private set
            {
                _Display_Heading1 = value;
                OnPropertyChanged(nameof(this.Display_Heading1));
            }
        }
        public object Display_Heading2
        {
            get => _Display_Heading2;
            private set
            {
                _Display_Heading2 = value;
                OnPropertyChanged(nameof(this.Display_Heading2));
            }
        }
        public object Display_Heading3
        {
            get => _Display_Heading3;
            private set
            {
                _Display_Heading3 = value;
                OnPropertyChanged(nameof(this.Display_Heading3));
            }
        }
        public object Display_Heading4
        {
            get => _Display_Heading4;
            private set
            {
                _Display_Heading4 = value;
                OnPropertyChanged(nameof(this.Display_Heading4));
            }
        }
        public object Display_Heading5
        {
            get => _Display_Heading5;
            private set
            {
                _Display_Heading5 = value;
                OnPropertyChanged(nameof(this.Display_Heading5));
            }
        }
        public object Display_Heading6
        {
            get => _Display_Heading6;
            private set
            {
                _Display_Heading6 = value;
                OnPropertyChanged(nameof(this.Display_Heading6));
            }
        }
        public object Display_Heading7
        {
            get => _Display_Heading7;
            private set
            {
                _Display_Heading7 = value;
                OnPropertyChanged(nameof(this.Display_Heading7));
            }
        }
        public object Display_Content
        {
            get => _Display_Content;
            private set
            {
                _Display_Content = value;
                OnPropertyChanged(nameof(Display_Content));
            }
        }
    }
    public partial class vmMarkdownContent
    {
        public override void SetInitialData()
        {
            this.Children = new ObservableCollection<vmMarkdownLine>();
            this.Children.CollectionChanged += Children_CollectionChanged;
        }

        public override void UpdateOriginData()
        {
        
        }

        public void SetParentHeading(vmMarkdownHeading value)
        {
            vmMarkdownHeading heading = value;
            while (heading != null)
            {
                switch (heading.HeadingType)
                {
                    case eLineType.Heading1: this.Display_Heading1 = heading.Display_Text; break;
                    case eLineType.Heading2: this.Display_Heading2 = heading.Display_Text; break;
                    case eLineType.Heading3: this.Display_Heading3 = heading.Display_Text; break;
                    case eLineType.Heading4: this.Display_Heading4 = heading.Display_Text; break;
                    case eLineType.Heading5: this.Display_Heading5 = heading.Display_Text; break;
                    case eLineType.Heading6: this.Display_Heading6 = heading.Display_Text; break;
                    case eLineType.Heading7: this.Display_Heading7 = heading.Display_Text; break;
                    default: break;
                }
                heading = heading.ParentHeading;
            }

            this.Parent = heading;
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    string value = string.Empty;
                    foreach (vmMarkdownLine line in this.Children)
                    {
                        if(line.Origin.Num < this.Origin.StartLineNumber) this.StartLineNumber = this.Origin.StartLineNumber = line.Origin.Num;
                        if (this.Origin.EndLineNumber < line.Origin.Num) this.EndLineNumber = this.Origin.EndLineNumber = line.Origin.Num;

                        value += line.Origin.Mark;
                        value += line.Origin.Text;
                        value += "\n";
                    }
                    this.Display_Content = value.Trim();
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                default:
                    break;
            }
        }
    }
}
