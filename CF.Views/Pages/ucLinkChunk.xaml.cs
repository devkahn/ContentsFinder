using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using CF.Helpers;
using CF.Models.DataModels.ChunkData;
using CF.Models.ViewModels.ChunkDataSetting;
using CF.Models.ViewModels.MarkDownSetting;
using CF.Models.ViewModels.PowerPointSetting;

namespace CF.Views.Pages
{
    /// <summary>
    /// ucLinkChunk.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ucLinkChunk : UserControl
    {
        private vmMarkdown _Markdown = null;
        public vmMarkdown Markdown
        {
            get => _Markdown;
            set
            {
                _Markdown = value;
                if (value == null) return;
                this.datagrid_OriginMD.ItemsSource = null;
                this.datagrid_OriginMD.ItemsSource = value.Lines;
            }
        }
        private vmChunk _Chunk = null;
        public vmChunk Chunk
        {
            get => _Chunk;
            set
            {
                _Chunk = value;
                ProgramValues.MaterialViewModel.SetChunk(value);
                this.datagrid_Chunk.ItemsSource = value.Data;
            }
        }


        public ucLinkChunk()
        {
            try
            {
                InitializeComponent();
                this.Chunk = new vmChunk(ProgramValues.CurrentMaterial.Chunk);
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }


        private void MoveLine()
        {
            bool isIndex = int.TryParse(this.txtbox_LineMove.Text, out int lnNum);
            while (!isIndex)
            {
                vmSlide selectedLine = this.datagrid_OriginMD.SelectedItem as vmLine;
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

        private void btn_MarkDownLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Markdown = MarkdownHelper.LoadMarkdonw();
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }

        private void btn_CunkLoad_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Chunk Data 열기";
            try
            {
                FileInfo fInfo = FileHelper.OpenFile(caption);
                if (fInfo == null) return;

                string jsonString = File.ReadAllText(fInfo.FullName);
                List<mChunkData> chunks = JsonHelper.ToObject<List<mChunkData>>(jsonString);
                ProgramValues.CurrentMaterial.Chunk.Datas.Clear();
                ProgramValues.MaterialViewModel.Chunk.Data.Clear();

                foreach (mChunkData chunk in chunks)
                {
                    chunk.ImageLinks = ChunkHelper.GetImageItems(chunk);
                    ProgramValues.CurrentMaterial.Chunk.Datas.Add(chunk);
                    this.Chunk.Data.Add(new vmChunkData(chunk));
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }

        private void btn_LnMove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void datagrid_OriginMD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void txtbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void txtbox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox tb = sender as TextBox;
                if (tb == null) return;

                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    switch (tb.Uid)
                    {
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBlock previeValue = new TextBlock() { Text = "NO CONTENTS" };

                vmChunkData selectedData = this.datagrid_Chunk.SelectedItem as vmChunkData;
                if (selectedData == null)
                {
                    this.txtblock_PreView = previeValue;
                    return;
                }

                RadioButton rBtn = sender as RadioButton;
                if (rBtn == null)
                {
                    this.txtblock_PreView = previeValue;
                    return;
                }

                bool isUidDigit = int.TryParse(rBtn.Uid, out int uidCode);
                if (!isUidDigit)
                {
                    this.txtblock_PreView = previeValue;
                    return;
                }

                //switch (uidCode)
                //{
                //    case 10: previeValue = TextHelper.HighlightMarkdown(selectedData.Origin.EmbeddingContents); break;
                //    case 20: previeValue = TextHelper.HighlightMarkdown(selectedData.Origin.RetrieveContents); break;
                //    case 30: previeValue = TextHelper.HighlightMarkdown(selectedData.Origin.GenerationContents); break;
                //    case 40: previeValue = TextHelper.HighlightMarkdown(selectedData.Origin.ReferenceContents); break;
                //    default: break;
                //}
                //this.txtblock_PreView = previeValue;

                switch (uidCode)
                {
                    case 10: this.txtblock_PreView.Text = TextHelper.HighlightMarkdown(selectedData.Origin.EmbeddingContents); break;
                    case 20: this.txtblock_PreView.Text = TextHelper.HighlightMarkdown(selectedData.Origin.RetrieveContents); break;
                    case 30: this.txtblock_PreView.Text = TextHelper.HighlightMarkdown(selectedData.Origin.GenerationContents); break;
                    case 40: this.txtblock_PreView.Text = TextHelper.HighlightMarkdown(selectedData.Origin.ReferenceContents); break;
                    default: break;
                }
                ;

            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }

        private void datagrid_Chunk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TextBlock previeValue = new TextBlock() { Text = "NO CONTENTS" };

                DataGrid currentControl = sender as DataGrid;
                if (currentControl == null)
                {
                    this.txtblock_PreView = previeValue;
                    return;
                }

                if (e.AddedItems == null)
                {
                    this.txtblock_PreView = previeValue;
                    return;
                }

                vmChunkData selectedData = e.AddedItems[0] as vmChunkData;
                if (selectedData == null)
                {
                    this.txtblock_PreView = previeValue;
                    return;
                }

                string uid = string.Empty;
                if (this.rBtn_Emedding.IsChecked == true) uid = this.rBtn_Emedding.Uid;
                if (this.rBtn_Retrieve.IsChecked == true) uid = this.rBtn_Retrieve.Uid;
                if (this.rBtn_Generation.IsChecked == true) uid = this.rBtn_Generation.Uid;
                if (this.rBtn_Reference.IsChecked == true) uid = this.rBtn_Reference.Uid;

                bool isUidDigit = int.TryParse(uid, out int uidCode);
                if (!isUidDigit)
                {
                    this.txtblock_PreView = previeValue;
                    return;
                }

                //switch (uidCode)
                //{
                //    case 10: previeValue = TextHelper.HighlightMarkdown(selectedData.Origin.EmbeddingContents); break;
                //    case 20: previeValue = TextHelper.HighlightMarkdown(selectedData.Origin.RetrieveContents); break;
                //    case 30: previeValue = TextHelper.HighlightMarkdown(selectedData.Origin.GenerationContents); break;
                //    case 40: previeValue = TextHelper.HighlightMarkdown(selectedData.Origin.ReferenceContents); break;
                //    default: break;
                //}
                //this.txtblock_PreView = previeValue;

                switch (uidCode)
                {
                    case 10: this.txtblock_PreView.Text = TextHelper.HighlightMarkdown(selectedData.Origin.EmbeddingContents); break;
                    case 20: this.txtblock_PreView.Text = TextHelper.HighlightMarkdown(selectedData.Origin.RetrieveContents); break;
                    case 30: this.txtblock_PreView.Text = TextHelper.HighlightMarkdown(selectedData.Origin.GenerationContents); break;
                    case 40: this.txtblock_PreView.Text = TextHelper.HighlightMarkdown(selectedData.Origin.ReferenceContents); break;
                    default: break;
                }


            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
    }
}
