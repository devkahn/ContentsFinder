using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CF.Commons;
using CF.Models.DataModels;
using CF.Models.DataModels.Markdown;


namespace CF.Models.ViewModels.MarkDownSetting
{
    public partial class vmMarkdown
    {
        private mMarkdown _Origin = null;

  
    }
    public partial class vmMarkdown : vmBase
    {
        public vmMarkdown(mMarkdown origin)  
        {
            SetInitialData();
            this.Origin = origin;
        }


        public mMarkdown Origin
        {
            get => _Origin;
            set
            {
                _Origin = value;
                if (value == null) return;

                SetLineInstance(value.Lines);
            }
        }

        public ObservableCollection<vmMarkdownLine> Lines { get; private set; }
        public ObservableCollection<vmMarkdownHeading> Headings { get; private set; }
        public ObservableCollection<vmMarkdownContent> Contents { get; private set; }
    }
    public partial class vmMarkdown 
    {
        public override void SetInitialData()
        {
            this.Lines = new ObservableCollection<vmMarkdownLine>();
            this.Headings = new ObservableCollection<vmMarkdownHeading>();
            this.Contents = new ObservableCollection<vmMarkdownContent>(); 
        }
        public override void UpdateOriginData()
        {

        }

        public void SetLineInstance(List<mMarkdownLine> lines)
        {
            if (lines == null) return;


            vmMarkdownHeading currentHead = null;

            vmMarkdownHeading heading1= null;
            vmMarkdownHeading heading2 = null;
            vmMarkdownHeading heading3 = null;
            vmMarkdownHeading heading4 = null;
            vmMarkdownHeading heading5 = null;
            vmMarkdownHeading heading6 = null;
            vmMarkdownHeading heading7 = null;


            vmMarkdownHeading[] headings = { heading1, heading2, heading3, heading4, heading5, heading6, heading7 };
            foreach (mMarkdownLine ln in lines)
            {
                vmMarkdownLine newLine = new vmMarkdownLine(ln);
                this.Lines.Add(newLine);

                if (newLine.LineType == eLineType.Empty || newLine.LineType == eLineType.None) continue;

                if(ln.LineTypeCode > 100)
                {
                    if(currentHead != null && currentHead.Conetnt != null) this.Contents.Add(currentHead.Conetnt);

                    vmMarkdownHeading newHeading = new vmMarkdownHeading(newLine);
                    this.Headings.Add(newHeading);
                    currentHead = newHeading;

                    switch (newLine.LineType)
                    {
                        case eLineType.Heading1:
                            heading1 = newHeading;
                            break;
                        case eLineType.Heading2:
                            heading2 = newHeading;
                            heading1.Children.Add(newHeading);
                            break;
                        case eLineType.Heading3:
                            heading3 = newHeading;
                            heading2.Children.Add(newHeading);
                            break;
                        case eLineType.Heading4:
                            heading4 = newHeading;
                            heading3.Children.Add(newHeading);
                            break;
                        case eLineType.Heading5:
                            heading5 = newHeading;
                            heading4.Children.Add(newHeading);
                            break;
                        case eLineType.Heading6:
                            heading6 = newHeading;
                            heading5.Children.Add(newHeading);
                            break;
                        case eLineType.Heading7:
                            heading7 = newHeading;
                            heading6.Children.Add(newHeading);
                            break;
                        default:
                            continue;
                    }

                    int start = ln.LineTypeCode - 100;
                    for (int i = start; i < headings.Count(); i++) headings[i] = null;
                }
                else
                {
                    if (currentHead.Conetnt == null) currentHead.SetConent(null);
                    currentHead.Conetnt.Children.Add(newLine);
                }
            }
        }
       
    }
}
