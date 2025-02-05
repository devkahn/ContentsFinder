using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CF.Commons;
using CF.Helpers;
using CF.Models.DataModels;
using CF.Models.ViewModels;
using CF.Models.ViewModels.PowerPointSetting;

namespace CF.Views.Pages
{
    /// <summary>
    /// ucTextModifier.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ucTextModifier : UserControl
    {
        private vmMaterial _Material = null;
        public vmMaterial Material
        {
            get => _Material;
            set
            {
                _Material = value;
                this.DataContext = value;
            }
        }


        public ucTextModifier()
        {
            InitializeComponent();
        }

        private void btn_textLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string caption = "Json Load";
                FileInfo fInfo = FileHelper.OpenFile(caption);
                if (fInfo == null) return;

                string jsonString = File.ReadAllText(fInfo.FullName);
                List<mSlide> slides = JsonHelper.ToObject<List<mSlide>>(jsonString);
                this.Material.PowerPoint.Slides.Clear();
                foreach (mSlide item in slides)
                {
                    //ProgramValues.CurrentMaterial.PowerPoint.Slides.Add(item);
                    this.Material.PowerPoint.Slides.Add(new vmSlide(item));
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void bnt_RemoveNoOutText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (vmSlide item in this.Material.PowerPoint.Slides)
                {
                    vmTextShape noOutText = item.Shapes.LastOrDefault();
                    if (noOutText == null) continue;

                    if (noOutText.Origin.Text.Contains(Defines.TEXT_NO_OUT))
                    {
                        item.Shapes.Remove(noOutText);
                    }
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void bnt_LastPageNum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (vmSlide item in this.Material.PowerPoint.Slides)
                {
                    vmTextShape pageText = item.Shapes.LastOrDefault();
                    if (pageText == null) continue;

                    if (int.TryParse(pageText.Origin.Text, out int page))
                    {
                        item.Shapes.Remove(pageText);
                    }
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void bnt_RemoveNoText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<vmSlide> emptyPag = new List<vmSlide>();
                foreach (vmSlide item in this.Material.PowerPoint.Slides)
                {
                    if (item.Shapes.Count() <= 0) emptyPag.Add(item);
                }
                foreach (vmSlide item in emptyPag)
                {
                    this.Material.PowerPoint.Slides.Remove(item);
                }

            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void bnt_RemoveTitleText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (vmSlide item in this.Material.PowerPoint.Slides)
                {
                    vmTextShape firstText = item.Shapes.FirstOrDefault();
                    if (firstText == null) continue;

                    if (firstText.Origin.Text.Contains("[철근콘크리트공사]"))
                    {
                        firstText.Origin.Text = firstText.Origin.Text.Replace("[철근콘크리트공사]", "").Trim();
                        firstText.SetDisplayText();
                    }
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void bnt_DivideLine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isDigit = int.TryParse(this.txtbox_TextitemIndex.Text, out int index);
                if (isDigit)
                {
                    foreach (vmSlide slide in this.Material.PowerPoint.Slides)
                    {
                        if (slide.Shapes.Count <= index) continue;

                        vmTextShape firstText = slide.Shapes[index];
                        if (firstText.Origin.Text.Contains("_"))
                        {
                            string[] lines = firstText.Origin.Text.Split('_');
                            mTextShape newText = new mTextShape();
                            newText.Left = firstText.Origin.Left;
                            newText.Top = firstText.Origin.Top + firstText.Origin.Height;
                            newText.Width = firstText.Origin.Width;
                            newText.Height = firstText.Origin.Height;
                            newText.Text = lines[1].Trim();
                            slide.Origin.TextShapes.Insert(1, newText);

                            slide.Shapes.Insert(1, new vmTextShape(newText));

                            firstText.Origin.Text = lines[0].Trim();
                            firstText.SetDisplayText();

                        }
                    }
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void bnt_RemoveSelectSlide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vmSlide selectedSlide = this.listbox_slidelist.SelectedItem as vmSlide;
                if (selectedSlide == null) return;

                this.Material.PowerPoint.Slides.Remove(selectedSlide);
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void bnt_ChangeText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string originText = this.txtbox_OriginText.Text;
                string targetText = this.txtbox_TargetText.Text;


                foreach (vmSlide slide in this.Material.PowerPoint.Slides)
                {
                    foreach (vmTextShape text in slide.Shapes)
                    {
                        if (text.Origin.Text.Contains(originText))
                        {
                            text.Origin.Text = text.Origin.Text.Replace(originText, targetText);
                            text.SetDisplayText();
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void bnt_RemoveOneChar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (vmSlide item in this.Material.PowerPoint.Slides)
                {
                    vmTextShape textShape = item.Shapes.FirstOrDefault();
                    if (textShape == null) continue;
                    if (textShape.Origin.Text.StartsWith("_"))
                    {
                        textShape.Origin.Text = textShape.Origin.Text.Substring(1).Trim();
                        textShape.SetDisplayText();
                    }
                }
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_textSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string caption = (sender as Button).Content.ToString();

                //List<mSlide> list = ProgramValues.CurrentMaterial.PowerPoint.Slides;

                List<mSlide> list = new List<mSlide>();
                foreach (vmSlide item in this.Material.PowerPoint.Slides)
                {
                    mSlide newSlide = new mSlide();
                    newSlide.Index = item.Origin.Index;
                    foreach (vmTextShape shape in item.Shapes)
                    {
                        newSlide.TextShapes.Add(shape.Origin);
                    }
                    list.Add(newSlide);
                }

                string jsonString = JsonHelper.ToJsonString(list);

                bool result = FileHelper.SaveTextFile(caption, jsonString);
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

                this.listbox_textList.ItemsSource = slide.Shapes;
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
    }
}
