using System;
using System.Collections.ObjectModel;
using System.Linq;
using CF.Commons;
using Newtonsoft.Json.Linq;


namespace CF.Models.ViewModels.MarkDownSetting
{
    public partial class vmTableContent
    {
        private mContent _Origing = null;

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

    public partial class vmTableContent : vmBase
    {
        public vmTableContent(mContent origin) 
        {
            SetInitialData();
            this.Origin = origin;
        }

        public mContent Origin
        {
            get => _Origing;
            private  set
            {
                _Origing = value;

                if (value == null) return;

                this.Display_PageNum = value.SlideNum;
                this.Display_Content = value.Value;

                string headingIdx = value.ParentId;
                while (string.IsNullOrEmpty(headingIdx))
                {
                    headingIdx = SetHeaderValueReturParent(headingIdx);
                }
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
        public int StartLineNumber { get; private set; } = -1;
        public int EndLineNumber { get; private set; } = -1;
        public ObservableCollection<vmLine> MapLines { get; private set; } 


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

    public partial class vmTableContent 
    {
        public override void SetInitialData()
        {
            this.MapLines = new ObservableCollection<vmLine>();
            this.MapLines.CollectionChanged += MapLines_CollectionChanged;
        }
        public override void UpdateOriginData()
        {

        }

        private string SetHeaderValueReturParent(string idx)
        {
            switch ((eLineType)value.HeadingTypeCode)
            {
                case eLineType.Heading1:
                    mHeading head1 = value as mHeading;
                    this.Display_Heading1 = head1.TextLine.LineContent.Substring(1).Trim();
                    return null;
                case eLineType.Heading2:
                    mHeading head2 = value as mHeading;
                    this.Display_Heading2 = head2.TextLine.LineContent.Substring(2).Trim();
                    return ProgramValues.CurrentMaterial.Markdown.Heading1s.Where(x => x.Id == head2.ParentId).FirstOrDefault();
                case eLineType.Heading3:
                    mHeading head3 = value as mHeading;
                    this.Display_Heading3 = head3.TextLine.LineContent.Substring(3).Trim();
                    return ProgramValues.CurrentMaterial.Markdown.Heading2s.Where(x => x.Id == head3.ParentId).FirstOrDefault();
                case eLineType.Heading4:
                    mHeading head4 = value as mHeading;
                    this.Display_Heading4 = head4.TextLine.LineContent.Substring(4).Trim();
                    return ProgramValues.CurrentMaterial.Markdown.Heading3s.Where(x => x.Id == head4.ParentId).FirstOrDefault();
                case eLineType.Heading5:
                    mHeading head5 = value as mHeading;
                    this.Display_Heading5 = head5.TextLine.LineContent.Substring(5).Trim();
                    return ProgramValues.CurrentMaterial.Markdown.Heading4s.Where(x => x.Id == head5.ParentId).FirstOrDefault();
                case eLineType.Heading6:
                    mHeading head6 = value as mHeading;
                    this.Display_Heading6 = head6.TextLine.LineContent.Substring(6).Trim();
                    return ProgramValues.CurrentMaterial.Markdown.Heading5s.Where(x => x.Id == head6.ParentId).FirstOrDefault();
                case eLineType.Heading7:
                default:
                    return null;
            }


        }
        public void SetProperties()
        {
            this.StartLineNumber = this.TextLines.Min(x => x.LineNum);
            this.EndLineNumber = this.TextLines.Max(x => x.LineNum);

            string temp = string.Empty;
            foreach (mTextLine item in this.TextLines)
            {
                temp += "\n";
                temp += item.LineContent;
            }
            this.Value = temp.Trim();
        }
        public void SetSlideNum(int num)
        {
            this.Origin.SlideNum = num;
            this.Display_PageNum = this.Origin.SlideNum;
        }
        public void SetCheckStatus(eCheckStatus status)
        {
            this.CheckStatus = status;
        }


        private void MapLines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (e.NewItems == null || e.NewItems.Count <= 0) return;
                    foreach (var item in e.NewItems)
                    {
                        vmLine addedItem = item as vmLine;
                        if (addedItem == null) continue;
                        addedItem.SetPageNum(this.Origin.SlideNum);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (e.OldItems == null || e.OldItems.Count < 0) return;
                    foreach (var item in e.OldItems)
                    {
                        vmLine removedItem = item as vmLine;
                        if (removedItem == null) continue;
                        removedItem.SetPageNum(-1);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
