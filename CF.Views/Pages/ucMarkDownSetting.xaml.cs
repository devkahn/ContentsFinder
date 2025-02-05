using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CF.Commons;
using CF.Helpers;
using CF.Models.ViewModels;
using CF.Models.ViewModels.MarkDownSetting;

namespace CF.Views.Pages
{
    /// <summary>
    /// ucMarkDownSetting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ucMarkDownSetting : UserControl
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

        public ucMarkDownSetting()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }

        private void btn_MDOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MarkdownHelper.OpenMarkdown(this.Markdown);
                this.Markdown = MarkdownHelper.LoadMarkdonw(this.Markdown);
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_MDLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Markdown = MarkdownHelper.LoadMarkdonw(this.Markdown);
            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }
        private void btn_MarkdownModeling_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ProgramValues.CurrentMaterial.Markdown.SetHeading(this.Markdown.Lines.ToList());

                //foreach (mContent item in ProgramValues.CurrentMaterial.Markdown.Contents)
                //{
                //    if(item == null) continue;
                //    vmTableContent newContent = new vmTableContent(item);
                //    newContent.SetProperties();

                //    this.Markdown.Contents.Add(newContent);
                //}
                //this.datagrid_TableMarkdown.ItemsSource = this.Markdown.Contents;
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
                    foreach (vmMarkdownLine item in this.Markdown.Lines) item.ChangeVisibility(item.LineType != eLineType.Empty);
                }
                else
                {
                    foreach (vmMarkdownLine item in this.Markdown.Lines) item.ChangeVisibility(true);
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
                foreach (vmMarkdownLine item in this.Markdown.Lines)
                {
                    item.AddIndent(isChecked);
                }

            }
            catch (Exception ee)
            {
                ErrorHelper.ShowError(ee);
            }
        }


    }
}
