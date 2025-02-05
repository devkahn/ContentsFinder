using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CF.Commons;
using CF.Helpers;
using CF.Models.ViewModels.MarkDownSetting;
using CF.Models.ViewModels.PowerPointSetting;

namespace CF.Views.Pages
{
    /// <summary>
    /// ucMappingData.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ucMappingData : UserControl
    {
        private vmPowerpoint _PowerPoint = null;
        private vmMarkdown _Markdown = null;
        private ObservableCollection<vmTableContent> _ContentList = new ObservableCollection<vmTableContent>();
        private ObservableCollection<vmLine> _LineList = new ObservableCollection<vmLine>();


        public vmPowerpoint PowerPoint
        {
            get => _PowerPoint;
            set
            {
                _PowerPoint = value;
                this.listbox_slidelist.ItemsSource = value.Slides;
                this.listbox_slidelist.SelectedIndex = 0;
            }
        }
        public vmMarkdown Markdown
        {
            get => _Markdown;
            set
            {
                _Markdown = value;
                this.ContentList = value.Contents;
                this.LineList = value.Lines;
            }
        }
        public ObservableCollection<vmTableContent> ContentList
        {
            get => _ContentList;
            set
            {
                _ContentList = value;
                this.datagrid_TableMarkdown.ItemsSource = value;
            }

        }
        public ObservableCollection<vmLine> LineList
        {
            get => _LineList;
            set
            {
                _LineList = value;
                this.datagrid_OriginMD.ItemsSource = value;
            }
        }


        public ucMappingData()
        {
            InitializeComponent();
        }


        private void MapSlideAndContent(vmSlide currentSlide)
        {
            List<vmTextShape> currentShapes = currentSlide.Shapes.ToList();

            /* 다음 슬라이드 찾기*/
            List<vmSlide> nextSlides = new List<vmSlide>();
            #region Next Slide 
            bool isDigit = int.TryParse(this.txtbox_checkRange.Text, out int range);
            if (!isDigit)
            {
                range = Convert.ToInt32(this.txtbox_checkRange.Uid);
                this.txtbox_checkRange.Text = 3.ToString();
            }
            vmSlide tempSlide = currentSlide;
            for (int i = 0; i < range; i++)
            {
                if (tempSlide == null) break;

                tempSlide = SlideHelper.GetNextSlide(tempSlide);
                if (tempSlide == null) continue;
                nextSlides.Add(tempSlide);
            }
            #endregion

            /* Mapping */
            List<vmTableContent> conVMList = this.Markdown.Contents.OrderBy(x => x.Origin.StartLineNumber).ToList();
            foreach (vmTableContent conVM in conVMList)
            {
                if (conVM.CheckStatus != eCheckStatus.None) continue;
                if (conVM.Origin.SlideNum != -1) continue;

                bool hasSlide = false;

                /* Parent 헤딩 텍스트 */
                string headingText = string.Empty;
                {
                    mHeading parentHeading = conVM.Origin.ParentHeading;
                    headingText = parentHeading.TextLine.LineContent.RemoveHeadingSymbol().Trim();
                    while (Defines.LIST_COMMON_TEXT.Contains(headingText))
                    {
                        parentHeading = HeadingHelper.GetParentHeading(parentHeading);
                        headingText = parentHeading.TextLine.LineContent.RemoveHeadingSymbol().Trim();
                    }

                    if (headingText.StartsWith("골조공사")) headingText = headingText.Replace("골조공사", "");
                    headingText = headingText.RemoveSpecialChar();
                    headingText = headingText.RemoveEmtpy();
                    headingText = headingText.ToUpper();
                }

                /* 현재 슬라이드에 포함되는지 판단*/
                int lastShapeIndex = 0;
                for (int curIndex = lastShapeIndex; curIndex < currentShapes.Count; curIndex++)
                {
                    vmTextShape shape = currentShapes[curIndex];
                    string shapeString = shape.Origin.Text.RemoveSpecialChar().RemoveEmtpy().ToUpper();

                    if (shapeString.Contains(headingText) || TextHelper.CalculateSimilary(headingText, shapeString) > 99)
                    //if(shapeString.Contains(headingText))
                    {
                        lastShapeIndex = curIndex;
                        conVM.SetSlideNum(currentSlide.Origin.Index);
                        this.Markdown.SetPageToLine(conVM);
                        shape.MapContent = conVM;
                        conVM.SetCheckStatus(eCheckStatus.Completed);
                        hasSlide = true;
                        break;
                    }
                }

                if (Defines.LIST_FINISH_HEADER.Contains(headingText)) break;
                if (hasSlide) continue;

                /* 다음 슬라이드에 포함되는지 판단*/
                foreach (vmSlide nSlide in nextSlides)
                {
                    bool hasNextSlide = false;
                    for (int sIndex = 0; sIndex < nSlide.Shapes.Count; sIndex++)
                    {
                        vmTextShape shape = nSlide.Shapes[sIndex];
                        string shapeString = shape.Origin.Text.RemoveSpecialChar().RemoveEmtpy();

                        if (shapeString.Contains(headingText) || TextHelper.CalculateSimilary(headingText, shapeString) > 99)
                        //if (text.Contains(headingText))
                        {
                            hasNextSlide = true;
                            hasSlide = true;
                            break;
                        }
                    }
                    if (hasNextSlide) break;
                }

                if (hasSlide) continue;
                //conVM.SetCheckStatus(eCheckStatus.Fail);
            }
        }
        private void MoveSlide()
        {
            bool isIndex = int.TryParse(this.txtbox_CurSlideIndex.Text, out int index);
            while (!isIndex)
            {
                vmSlide curSlide = this.listbox_slidelist.SelectedItem as vmSlide;
                if (curSlide == null)
                {
                    return;
                }
                else
                {
                    this.txtbox_CurSlideIndex.Text = curSlide.Origin.Index.ToString();
                }
                isIndex = true;
            }

            vmSlide sameSlide = this.PowerPoint.Slides.Where(x => x.Origin.Index == index).FirstOrDefault();
            if (sameSlide == null)
            {
                string msg = string.Format("{0} 번 슬라이드가 없습니다.", index);
                MessageHelper.ShowErrorMessage("Slide 이동", msg);
                return;
            }
            else
            {
                this.listbox_slidelist.SelectedItem = sameSlide;
                this.listbox_slidelist.ScrollIntoView(this.listbox_slidelist.SelectedItem);
                var scrollViewer = ControlHelper.FindVisualChild<ScrollViewer>(this.listbox_slidelist);
                scrollViewer.ScrollToBottom();
                scrollViewer.ScrollToVerticalOffset(0);
            }
        }
        private void MoveLine()
        {
            bool isIndex = int.TryParse(this.txtbox_LineMove.Text, out int lnNum);
            while (!isIndex)
            {
                vmLine selectedLine = this.datagrid_OriginMD.SelectedItem as vmLine;
                if (selectedLine == null)
                {
                    return;
                }
                else
                {
                    this.txtbox_LineMove.Text = selectedLine.Origin.LineNum.ToString();
                }
                isIndex = true;
            }

            vmLine sameLine = this.Markdown.Lines.Where(x => x.Origin.LineNum == lnNum).FirstOrDefault();
            if (sameLine == null)
            {
                string msg = string.Format("{0} 번 Ln이 없습니다.", lnNum);
                MessageHelper.ShowErrorMessage("Line 이동", msg);
                return;
            }
            else
            {
                this.datagrid_OriginMD.SelectedItem = sameLine;
                this.datagrid_OriginMD.ScrollIntoView(this.datagrid_OriginMD.SelectedItem);
                var scrollViewer = ControlHelper.FindVisualChild<ScrollViewer>(this.datagrid_OriginMD);
                scrollViewer.ScrollToBottom();
                scrollViewer.ScrollToVerticalOffset(0);
            }
        }
        private void MoveContents(int index)
        {
            if (this.Markdown == null) return;
            var sameContents = this.ContentList.Where(x => x.Origin.SlideNum == index).ToList();
            if (sameContents == null) return;

            this.datagrid_TableMarkdown.SelectedItems.Clear();
            for (int i = 0; i < sameContents.Count(); i++)
            {
                this.datagrid_TableMarkdown.SelectedItems.Add(sameContents[i]);
                if (i == sameContents.Count() - 1) this.datagrid_TableMarkdown.ScrollIntoView(sameContents[i]);
            }
        }
        private void FilterPowerPointText(vmSlide selectedSlide)
        {
            string uid = string.Empty;
            foreach (RadioButton rBtn in this.stackPanel_PPTTextFilter.Children)
            {
                if (rBtn.IsChecked != true) continue;

                uid = rBtn.Uid;
                break;
            }

            this.listbox_textList.ItemsSource = null;
            bool isUidDigit = int.TryParse(uid, out int uidCode);
            switch (uidCode)
            {
                case 220:
                    this.listbox_textList.ItemsSource = selectedSlide.Shapes.Where(x => x.HasMapContent);
                    break;
                case 230:
                    List<vmTextShape> shapes = new List<vmTextShape>();

                    var headings = selectedSlide.Shapes.Where(x => x.HasMapContent);
                    foreach (vmTextShape hd in headings)
                    {
                        int index = selectedSlide.Shapes.IndexOf(hd);
                        if (index != 0)
                        {
                            vmTextShape preShape = selectedSlide.Shapes[index - 1];
                            shapes.Add(preShape);
                        }

                        shapes.Add(hd);

                        if (index != selectedSlide.Shapes.Count - 1)
                        {
                            vmTextShape nextShape = selectedSlide.Shapes[index + 1];
                            shapes.Add(nextShape);
                        }
                    }
                    this.listbox_textList.ItemsSource = shapes;

                    break;
                case 240:
                    this.listbox_textList.ItemsSource = selectedSlide.Shapes.Where(x => !x.HasMapContent);
                    break;
                default:
                    this.listbox_textList.ItemsSource = selectedSlide.Shapes;
                    break;
            }

        }







        private void btn_MarkDownLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vmMarkdown md = ProgramValues.MaterialViewModel.Markdown;
                this.Markdown = md;

            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_PowerPointLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vmPowerpoint ppt = ProgramValues.MaterialViewModel.PowerPoint;
                this.PowerPoint = ppt;
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_MappingOne_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vmSlide selectedSlide = this.listbox_slidelist.SelectedItem as vmSlide;
                if (selectedSlide == null) return;

                List<vmTextShape> shapes = selectedSlide.Shapes.ToList();

                var remainLines = this.Markdown.Lines.Where(x => x.Origin.PageNum == -1).OrderBy(x => x.Origin.LineNum);

                vmLine last = null;
                int cnt = 0;
                foreach (vmLine ln in remainLines.Where(x => x.IsHeading))
                {
                    string headingText = ln.Origin.LineContent.RemoveHeadingSymbol().Trim();
                    if (headingText.StartsWith("골조공사")) headingText = headingText.Replace("골조공사", "");
                    headingText = headingText.RemoveSpecialChar();
                    headingText = headingText.RemoveEmtpy();

                    for (int i = cnt; i < selectedSlide.Shapes.Count; i++)
                    {
                        vmTextShape shape = selectedSlide.Shapes[i];
                        string text = shape.Origin.Text.RemoveSpecialChar().RemoveEmtpy();
                        if (text.Contains(headingText))
                        {
                            cnt = i;
                            last = ln;
                            break;
                        }
                    }

                }


            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_MappingOne_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                vmSlide selectedSlide = this.listbox_slidelist.SelectedItem as vmSlide;
                if (selectedSlide == null) return;
                vmSlide nextSlide = SlideHelper.GetNextSlide(selectedSlide);

                List<vmTextShape> shapes = selectedSlide.Shapes.ToList();

                int startNum = 1;
                int lastNum = -1;

                List<vmTableContent> contents = this.Markdown.Contents.OrderBy(x => x.Origin.StartLineNumber).ToList();
                bool isFinish = false;
                foreach (vmTableContent con in contents)
                {
                    isFinish = false;

                    mHeading parentHeading = con.Origin.ParentHeading;
                    string headingText = parentHeading.TextLine.LineContent.RemoveHeadingSymbol().Trim();
                    if (headingText.StartsWith("골조공사")) headingText = headingText.Replace("골조공사", "");
                    headingText = headingText.RemoveSpecialChar();
                    headingText = headingText.RemoveEmtpy();



                    int cnt = 0;


                    for (int i = cnt; i < selectedSlide.Shapes.Count; i++)
                    {
                        vmTextShape shape = selectedSlide.Shapes[i];
                        string text = shape.Origin.Text.RemoveSpecialChar().RemoveEmtpy();
                        if (text.Contains(headingText))
                        {
                            cnt = i;
                            lastNum = con.Origin.EndLineNumber;
                            isFinish = true;
                            break;
                        }
                    }

                    if (isFinish) continue;

                    cnt = 0;
                    for (int i = cnt; i < nextSlide.Shapes.Count; i++)
                    {
                        vmTextShape shape = nextSlide.Shapes[i];
                        string text = shape.Origin.Text.RemoveSpecialChar().RemoveEmtpy();
                        if (text.Contains(headingText))
                        {
                            lastNum = con.Origin.ParentHeading.TextLine.LineNum - 1;
                            break;
                        }
                    }
                    break;
                }


                for (int i = startNum; i <= lastNum; i++)
                {
                    this.Markdown.Lines[i - 1].SetPageNum(selectedSlide.Origin.Index);
                }



            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_MappingOne_Click3(object sender, RoutedEventArgs e)
        {
            try
            {
                vmSlide currentSlide = this.listbox_slidelist.SelectedItem as vmSlide;
                if (currentSlide == null) return;
                MapSlideAndContent(currentSlide);

            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_MappingAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int contentCnt = 0;
                int startNum = 1;
                int lastNum = -1;

                for (int sNum = 0; sNum < this.PowerPoint.Slides.Count; sNum++)
                {
                    this.listbox_slidelist.SelectedIndex = sNum;


                    vmSlide selectedSlide = this.listbox_slidelist.SelectedItem as vmSlide;
                    if (selectedSlide == null) return;
                    vmSlide nextSlide = SlideHelper.GetNextSlide(selectedSlide);

                    List<vmTextShape> shapes = selectedSlide.Shapes.ToList();



                    List<vmTableContent> contents = this.Markdown.Contents.OrderBy(x => x.Origin.StartLineNumber).ToList();
                    bool isFinish = false;

                    for (int conNum = contentCnt; conNum < contents.Count; conNum++)
                    {
                        vmTableContent con = contents[conNum];

                        isFinish = false;

                        mHeading parentHeading = con.Origin.ParentHeading;
                        string headingText = parentHeading.TextLine.LineContent.RemoveHeadingSymbol().Trim();
                        while (Defines.LIST_COMMON_TEXT.Contains(headingText))
                        {
                            parentHeading = HeadingHelper.GetParentHeading(parentHeading);
                            headingText = parentHeading.TextLine.LineContent.RemoveHeadingSymbol().Trim();
                        }

                        if (headingText.StartsWith("골조공사")) headingText = headingText.Replace("골조공사", "");
                        headingText = headingText.RemoveSpecialChar();
                        headingText = headingText.RemoveEmtpy();
                        headingText = headingText.ToUpper();


                        if (headingText == "통합설계체크리스트")
                        {

                        }



                        int cnt = 0;
                        for (int i = cnt; i < selectedSlide.Shapes.Count; i++)
                        {
                            vmTextShape shape = selectedSlide.Shapes[i];
                            string text = shape.Origin.Text.RemoveSpecialChar().RemoveEmtpy().ToUpper();
                            if (text.Contains(headingText))
                            {
                                cnt = i;
                                lastNum = con.Origin.EndLineNumber;
                                isFinish = true;
                                break;
                            }
                        }

                        if (Defines.LIST_FINISH_HEADER.Contains(headingText))
                        {
                            contentCnt = conNum + 1;
                            lastNum = con.Origin.EndLineNumber;
                            break;
                        }
                        if (isFinish) continue;

                        cnt = 0;

                        bool isNoHeader = true;
                        for (int i = cnt; i < nextSlide.Shapes.Count; i++)
                        {
                            vmTextShape shape = nextSlide.Shapes[i];
                            string text = shape.Origin.Text.RemoveSpecialChar().RemoveEmtpy().ToUpper();
                            if (text.Contains(headingText))
                            {
                                vmTableContent preContent = contents[conNum - 1];


                                lastNum = preContent.Origin.EndLineNumber;
                                isNoHeader = false;
                                contentCnt = conNum;
                                break;
                            }
                        }
                        if (isNoHeader)
                        {
                            for (int lnNum = startNum; lnNum <= lastNum; lnNum++)
                            {
                                this.Markdown.Lines[lnNum - 1].SetPageNum(selectedSlide.Origin.Index);
                            }

                            string eMsg = string.Empty;
                            eMsg += "마지막 슬라이드 : ";
                            eMsg += selectedSlide.Origin.Index;
                            eMsg += "\n";

                            eMsg += "마지막 Ln :";
                            eMsg += lastNum;
                            eMsg += "\n";

                            eMsg += "찾지 못한 헤더 : ";
                            eMsg += headingText;

                            MessageBox.Show(eMsg, "Not found", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        break;
                    }


                    for (int lnNum = startNum; lnNum <= lastNum; lnNum++)
                    {
                        this.Markdown.Lines[lnNum - 1].SetPageNum(selectedSlide.Origin.Index);
                    }
                    startNum = lastNum + 1;
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_MappingAll_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int slideIndex = 0; slideIndex < this.PowerPoint.Slides.Count; slideIndex++)
                {
                    vmSlide currentSlide = this.PowerPoint.Slides[slideIndex];
                    if (currentSlide == null) return;

                    Debug.WriteLine(string.Format("--- {0}번 슬라이드 검토중 ... ---", currentSlide.Origin.Index));
                    MapSlideAndContent(currentSlide);
                }

                Debug.WriteLine(string.Format("--- Mapping Completed ---"));
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_MappingContinue_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn_SlideMove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MoveSlide();
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_LnMove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MoveLine();
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_SetCurrentPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vmSlide selectedPage = this.listbox_slidelist.SelectedItem as vmSlide;
                if (selectedPage == null) return;

                vmTextShape selectedShape = this.listbox_textList.SelectedItem as vmTextShape;
                if (selectedShape == null) return;

                vmTableContent selectedContent = this.datagrid_TableMarkdown.SelectedItem as vmTableContent;
                if (selectedContent == null) return;

                int pg = selectedPage.Origin.Index;
                selectedContent.SetSlideNum(pg);
                selectedShape.MapContent = selectedContent;
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_Finish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn == null) return;

                vmSlide selectedSlide = this.listbox_slidelist.SelectedItem as vmSlide;
                if (selectedSlide == null) return;

                bool isUidDigit = int.TryParse(btn.Uid, out int uidCode);
                if (isUidDigit)
                {
                    selectedSlide.SetCheckStatus((eCheckStatus)uidCode);
                }

                if (uidCode == 10)
                {
                    int index = this.listbox_slidelist.SelectedIndex;
                    this.listbox_slidelist.SelectedIndex = index + 1;
                    this.listbox_slidelist.ScrollIntoView(this.listbox_slidelist.SelectedItem);
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_MaterialSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string caption = (sender as Button).Content.ToString();

                string filename = "040_Material_" + DateTime.Now.ToString("yymmddHHMMss");
                FileInfo fInfo = FileHelper.GetSavePath(caption, filename);
                if (fInfo == null) return;

                vmMaterials currentMaterial = ProgramValues.MaterialViewModel;
                currentMaterial.UpdateOriginData();
                mMaterial materialData = currentMaterial.Origin;
                string jsonString = JsonHelper.ToJsonString(materialData);

                bool result = FileHelper.SaveTextFile(caption, jsonString, fInfo);
                if (result)
                {
                    string sMsg = "데이터 저장에 성공하였습니다.";
                    MessageHelper.ShowSuccessMessage(caption, sMsg);
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }

        private void datagrid_TableMarkdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems == null || e.AddedItems.Count <= 0) return;

                vmTableContent selectedItem = e.AddedItems[0] as vmTableContent;
                if (selectedItem == null) return;

                mTextLine firstLine = selectedItem.Origin.TextLines.FirstOrDefault();
                if (firstLine == null) return;

                vmLine sameLine = this.Markdown.Lines.Where(x => x.Origin.LineNum == firstLine.LineNum).FirstOrDefault();
                if (sameLine == null) return;

                //this.datagrid_OriginMD.SelectedItem = sameLine;
                //this.datagrid_OriginMD.ScrollIntoView(sameLine);


            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void datagrid_OriginMD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems == null) return;

                vmLine selectedLine = e.AddedItems[0] as vmLine;
                if (selectedLine == null) return;

                this.txtbox_LineMove.Text = selectedLine.Origin.LineNum.ToString();

                var scrollViewer = ControlHelper.FindVisualChild<ScrollViewer>(this.datagrid_OriginMD);
                scrollViewer.ScrollToLeftEnd();

            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void DataGridRow_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {

            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }

        private void listbox_slidelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.listbox_textList.ItemsSource = null;
                this.listbox_textList.Items.Clear();

                ListBox currentControl = sender as ListBox;
                if (currentControl == null) return;

                if (e.AddedItems == null || e.AddedItems.Count <= 0) return;

                vmSlide slide = e.AddedItems[0] as vmSlide;
                if (slide == null) return;

                FilterPowerPointText(slide);


                int index = slide.Origin.Index;
                this.txtbox_CurSlideIndex.Text = index.ToString();

                if (this.toggle_ContentsBySlide.IsChecked == true)
                {
                    var filtered = this.Markdown.Contents.Where(x => x.Origin.SlideNum == index);
                    this.ContentList = new ObservableCollection<vmTableContent>(filtered);
                }


                MoveContents(slide.Origin.Index);
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }

        private void toggle_EmptyCollapse_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ToggleButton currentControl = sender as ToggleButton;
                if (currentControl == null) return;

                bool isChecked = currentControl.IsChecked == true;
                if (isChecked)
                {
                    foreach (vmLine item in this.Markdown.Lines) item.ChangeVisibility(!item.IsEmepty);
                }
                else
                {
                    foreach (vmLine item in this.Markdown.Lines) item.ChangeVisibility(true);
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void toggle_IndentOnOff_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ToggleButton currentControl = sender as ToggleButton;
                if (currentControl == null) return;

                bool isChecked = currentControl.IsChecked == true;
                foreach (vmLine item in this.Markdown.Lines)
                {
                    item.AddIndent(isChecked);
                }

            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void toggle_ContentsBySlide_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ToggleButton tBtn = sender as ToggleButton;
                if (tBtn == null) return;

                int index = (this.listbox_slidelist.SelectedItem as vmSlide).Origin.Index;
                bool isChecked = tBtn.IsChecked == true;
                if (isChecked)
                {
                    this.toggle_NoneContents.IsChecked = false;
                    this.toggle_NoneContents.IsEnabled = false;

                    var filteredList = this.Markdown.Contents.Where(x => x.Origin.SlideNum == index);
                    this.ContentList = new ObservableCollection<vmTableContent>(filteredList);
                }
                else
                {
                    this.toggle_NoneContents.IsEnabled = true;
                    this.ContentList = this.Markdown.Contents;
                }

                MoveContents(index);
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void toggle_NoneContents_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ToggleButton tBtn = sender as ToggleButton;
                if (tBtn == null) return;

                bool isChecked = tBtn.IsChecked == true;
                if (isChecked)
                {
                    this.ContentList = new ObservableCollection<vmTableContent>(this.Markdown.Contents.Where(x => x.Origin.SlideNum == -1));
                }
                else
                {
                    this.ContentList = this.Markdown.Contents;
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void txtbox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                if (!TextHelper.IsTextNuberic(e.Text)) e.Handled = true;
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void txtbox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            try
            {
                if (!TextHelper.IsTextNuberic(e.DataObject.GetData(typeof(string)) as string)) e.CancelCommand();
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void txtbox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                TextBox tb = sender as TextBox;
                if (tb == null) return;

                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    switch (tb.Uid)
                    {
                        case "PAGEMOVE": MoveSlide(); break;
                        case "LINEMOVE": MoveLine(); break;
                        default: return;
                    }
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }


        private void rBtn_pptFilter_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.listbox_textList == null) return;

                vmSlide selectedSlide = this.listbox_slidelist.SelectedItem as vmSlide;
                if (selectedSlide == null) return;

                FilterPowerPointText(selectedSlide);
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void rBtn_ContentFilter_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
    }
}
