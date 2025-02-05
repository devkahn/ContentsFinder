using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CF.Commons;
using CF.Models.DataModels;

namespace CF.Models.ViewModels.MarkDownSetting
{
    public partial class vmMarkdownLine 
    {
        protected mMarkdownLine _Origin = null;

        private bool _IsVisible = true;

        private object _Display_PageNum = null;
        private object _Display_LineNum = null;
        private object _Display_LineValue = null;
    }

    public partial class vmMarkdownLine : vmBase
    {
        public vmMarkdownLine(mMarkdownLine origin)
        {
            SetInitialData();
            this.Origin = origin;
        }

        public mMarkdownLine Origin
        {
            get => this._Origin;
            protected set
            {
                this._Origin = value;
                if (value == null) return;

                this.LineType = (eLineType)value.LineTypeCode;

                this.Display_PageNum = value.Page;
                this.Display_LineNum = value.Num;
                this.Display_LineValue = value.Mark + value.Text;
            }
        }

        /* Status */
        public eLineType LineType { get; private set; } = eLineType.None;
        public bool IsVisible
        {
            get => _IsVisible;
            private set
            {
                _IsVisible = value;
                OnPropertyChanged(nameof(RowVisibility));
            }
        }


        /* Variable */
        

        /* Style */
        public Visibility RowVisibility => this.IsVisible? Visibility.Visible : Visibility.Collapsed;   
        

        /* Display */
        public object Display_PageNum
        {
            get => _Display_PageNum;
            private set
            {
                _Display_PageNum = value;
                OnPropertyChanged(nameof(Display_PageNum));
            }
        }
        public object Display_LineNum
        {
            get => this._Display_LineNum;
            private set
            {
                this._Display_LineNum = value;
                OnPropertyChanged(nameof(Display_LineNum));
            }
        }
        public object Display_LineValue
        {
            get => this._Display_LineValue;
            private set
            {
                this._Display_LineValue = value;
                OnPropertyChanged(nameof(Display_LineValue));
            }
        }
    }

    public partial class vmMarkdownLine
    {
        public override void SetInitialData()
        {
            
        }
        public override void UpdateOriginData()
        {

        }
        public void ChangeVisibility(bool isVisible)
        {
            this.IsVisible = isVisible;
        }

     
        public void SetPageNum(int num)
        {
            this.Display_PageNum = this.Origin.Page = num;
        }
        public void AddIndent(bool addIndent)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            if (addIndent)
            {
                #region 이전 코드
                //int cnt = 0;
                //switch (this.LineType)
                //{
                //    case eLineType.Heading2: cnt = 2; break;
                //    case eLineType.Heading3: cnt = 4; break;
                //    case eLineType.Heading4: cnt = 6; break;
                //    case eLineType.Heading5: cnt = 8; break;
                //    case eLineType.Heading6: cnt = 10; break;
                //    case eLineType.NormalText: cnt = 12; break;
                //    default: cnt = 0; break;
                //}

                //string value = string.Empty;
                //for (int i = 0; i < cnt; i++)
                //{
                //    value += " ";
                //    //if (i % 2 == 0) value += "|";
                //}
                //value += this.Origin.LineContent;
                #endregion
         

                int indentCnt = 0;
                switch (this.LineType)
                {
                    case eLineType.Heading2: indentCnt = 1; break;
                    case eLineType.Heading3: indentCnt = 2; break;
                    case eLineType.Heading4: indentCnt = 3; break;
                    case eLineType.Heading5: indentCnt = 4; break;
                    case eLineType.Heading6: indentCnt = 5; break;
                    case eLineType.Heading7: indentCnt = 6; break;
                    case eLineType.NormalText: indentCnt = 6; break;
                    default: break;
                }

                for (int i = 0; i < indentCnt; i++) panel.Children.Add(new Border());
            }

            panel.Children.Add(new TextBlock() { Text = this.Origin.Mark });
            panel.Children.Add(new TextBlock() { Text = this.Origin.Text });
            this.Display_LineValue = panel;
        }
    }
}
